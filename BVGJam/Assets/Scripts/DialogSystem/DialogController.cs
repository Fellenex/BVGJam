using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {

    public static String PLAYER_STRING = "Pal";

    //Prevent the player from advancing the conversation arbitrarily when they have to make a choice
    bool PLAYER_CHOOSING = false;

    //A handle to the graphics script on the panel.
    //Should be set in the inspector
    public DialogGraphics graphics;
    public StoryConditionManager conditionManager;

    //TODO should make a model for an "Active Conversation" with this info.
    Conversation activeConversation;
    Conversation_State activeState;
    Conversation_Statement activeStatement;
    int activeStatementIndex;

    public static event Action<Conversation, bool> StopConversationEvent;

    public void Start() {
        graphics.SetNextStateEvent += ChooseOption;
    }

    void Update() {
        //Only let the player advance the state if there is an active conversation, and they are not currently choosing
        if (!PLAYER_CHOOSING && activeConversation != null &&
            (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
            Debug.Log("Player-triggered advancing");
            AdvanceConversation();
        }

        //DEBUG ONLY. TODO REMOVE.
        if (Input.GetKeyDown(KeyCode.X)) {
            StopConversation();
        }
    }

    //Takes a conversation object and starts delivering it to the user
    //Initializes relevant 
    public void StartConversation(Conversation _conversation) {
        Debug.Log("DialogController::StartConversation() has begun ("+_conversation.id+")");

        activeConversation = _conversation;
        activeState = _conversation.getFirstState();
        activeStatementIndex = 0;

        graphics.activate();
        graphics.initializeConversation(_conversation.getNPCName());
        NextStatement(activeState);
    }

    public void StopConversation(){
        Debug.Log("DialogController::StopConversation() has begun");

        //Disable the conversation window
        graphics.deactivate();

        Conversation previousConversation = activeConversation;
        bool isFinalState = activeConversation.isAcceptingState(activeState.index);

        activeConversation = null;
        activeState = null;
        activeStatement = null;
        activeStatementIndex = 0;

        //Everything has been wrapped up, so hand back control to the DialogManager
        StopConversationEvent?.Invoke(previousConversation, isFinalState);
    }

    private void AdvanceConversation() {
        Debug.Log("AdvanceConversation()");
        if (activeState.hasMoreStatements(activeStatementIndex)) {
            NextStatement(activeState);
        } else {
            //No more statements - see if we have a transition that we can take to another state
            Conversation_Transition tr = activeConversation.getTransitionByIndex(activeState.index);
        
            if (tr != null) {
                //Handle the transition
                StartTransition(tr);
            } else {
                //No more statements available, and no transitions available
                Debug.Log("Stopping conversation - no more statements available");
                StopConversation();
            }
        }
    }

    private void StartTransition(Conversation_Transition _transition) {
        //If we had a colour trigger state before, we need to reset the background
        //TODO move this somewhere else. ideally DialogController is system-agnostic
        graphics.resetBackgroundColour();

        //A few special cases if there's only one transition option.
        if (_transition.options.Count() == 1) {
            if (String.IsNullOrEmpty(_transition.options[0].optionText)) {
                //No option text to show, and only one choice, so just make that choice
                //Sometimes we have a transition just to grant the player a condition
                ChooseOption(_transition.options[0]);
            } else {
                //Option text we want to show, but only one choice. Still let them choose it.
                PlayerChoosing(_transition);
            }
        } else {
            PlayerChoosing(_transition);
        }
    }

    //The player has chosen a speech option - update the conversation to reflect this choice
    //If the player chose a special coloured trigger, then show the dramatic dialog before advancing the conversation
    private void ChooseOption(Conversation_Option _option) {
        Debug.Log("ChooseOption()");

        conditionManager.HandleTriggers(_option.triggers);
        Conversation_Trigger specialTrigger = conditionManager.getSpecialTrigger(_option.triggers);

        //Get the graphics to update - this is a special state now
        if (specialTrigger != null) {
            graphics.handleTrigger(specialTrigger);
        }

        //Update the active state+statement index, then try to get the first statement from that state
        activeState = activeConversation.getStateByIndex(_option.target);
        activeStatementIndex = 0;
        AdvanceConversation();
    }

    //Move onto the next statement in the current state
    private void NextStatement(Conversation_State _state) {
        PLAYER_CHOOSING = false;
        Debug.Log("Starting next statement of " + _state.index + " (starting the " + activeStatementIndex + "th statement)");
        activeStatement = _state.statements[activeStatementIndex];
        Debug.Log("Statement belongs to " + activeStatement.speaker);

        if (activeStatement.speaker == PLAYER_STRING) {
            Debug.Log("Player statement");
            graphics.playerStatement(activeStatement);
        } else if (!String.IsNullOrEmpty(activeStatement.speaker)) {
            Debug.Log("NPC statement");
            graphics.npcStatement(activeStatement);
        } else {
            Debug.Log("Speakerless statement");
            graphics.speakerlessStatement(activeStatement);
        }

        //Get ready for the next statement
        activeStatementIndex += 1;
    }

    //Hand control over to the player for choosing between a few options
    private void PlayerChoosing(Conversation_Transition _transition) {
        PLAYER_CHOOSING = true;

        //Only show the options to which the player currently has access
        graphics.playerIsChoosing(getCurrentlyAvailableOptions(_transition));
    }


    //A small helper function to filter out options for which a player doesn't meet the conditions
    private List<Conversation_Option> getCurrentlyAvailableOptions(Conversation_Transition _transition) {
        return _transition.options.Where(x => playerMeetsOptionConditions(x)).ToList();
    }

    //Checks to see if player meets the conditions for a given transition option
    private bool playerMeetsOptionConditions(Conversation_Option _transitionOption) {
        Debug.Log("Condition(s) Required: " + _transitionOption.conditions);
        foreach (string condition in _transitionOption.conditions) {
            if (!conditionManager.hasMetCondition(condition)){
                //We have found a precondition the player does not meet.
                return false;
            }
        }
        //If we have not yet returned false, then we have met all the preconditions
        return true;
    }
}
