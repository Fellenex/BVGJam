using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {

    public static string PLAYER_STRING = "Pal";
    public static string GOOD_QUALITY = "good";
    public static string BAD_QUALITY = "bad";


    //Prevent the player from advancing the conversation arbitrarily when they have to make a choice
    bool PLAYER_CHOOSING = false;

    public DialogGraphics graphics;        // a handle to update the graphics


    //TODO should make a model for an "Active Conversation" with this info.
    Conversation activeConversation;
    Conversation_State activeState;
    Conversation_Statement activeStatement;
    int activeStatementIndex;

    public static event Action<string> StopConversationEvent;

    public void Start() {
        DialogGraphics.instance.SetNextStateEvent += ChooseOption;
    }

    void Update() {
        //Only let the player advance the state if they are not currently choosing
        if (!PLAYER_CHOOSING && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
            AdvanceConversation();
        }

        //DEBUG ONLY. TODO REMOVE.
        if (Input.GetKeyDown(KeyCode.X)) {
            DialogManager.instance.OnStopConversation(activeState.index);
        }
    }

    //Takes a conversation object and starts delivering it to the user
    //Initializes relevant 
    public void StartConversation(Conversation _conversation) {
        Debug.Log("DialogController::StartConversation() has begun ("+_conversation.id+")");

        activeConversation = _conversation;
        activeState = _conversation.getFirstState();

        StoryConditions.StartConversation(_conversation.getNPCName(), _conversation.id);
        graphics.initializeConversation(_conversation.getNPCName());
        startStatements(activeState);
    }

    public void StopConversation(){
        Debug.Log("DialogController::StopConversation() has begun");

        if (activeConversation.isAcceptingState(activeState.index)) {
            //We've finished this conversation.
            StoryConditions.finishConversation(activeConversation.id, "Paladin");
        }
                                                                                                                                                                                                                                                                                                            
        //Everything has been wrapped up, so hand back control to the DialogManager
        StopConversationEvent?.Invoke(activeState.index);
    }

    private void AdvanceConversation(){
        if (activeState.hasMoreStatements(activeStatementIndex)) {
            nextStatement(activeState);
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
        //A few special cases if there's only one transition option.
        if (_transition.options.Count() == 1) {
            if (String.IsNullOrEmpty(_transition.options[0].optionText)) {
                //No option text to show, and only one choice, so just make that choice
                ChooseOption(_transition.options[0]);
            } else {
                //Option text we want to show, but only one choice. Still let them choose it.
                playerChoosing(_transition);
            }
        } else {
            playerChoosing(_transition);
        }
    }

    //The player has chosen a speech option - update the conversation to reflect this choice
    private void ChooseOption(Conversation_Option _option) {
        bool showDramaticDialog = false;
        Conversation_Trigger specialTrigger = null;

        //If there were triggers, then cause them here.
        foreach (Conversation_Trigger trigger in _option.triggers) {

            //Track the trigger's name
            StoryConditions.playerHasMetCondition(trigger.text);

            if (trigger.isSpecialTrigger()) {
                showDramaticDialog = true;
                specialTrigger = trigger;
            }
        }

        //If we have a special trigger, then we have some additional 
        if (specialTrigger != null) {
            StoryConditions.playerHasMetCondition(specialTrigger.colour);
            if (specialTrigger.quality == GOOD_QUALITY) {
                StoryConditions.goodColoursCount++;
            } else if (specialTrigger.quality == BAD_QUALITY) {
                StoryConditions.badColoursCount++;
            } else {
                Debug.LogError("DialogController::ChooseOption() bad quality (" + specialTrigger.quality + ")");
            }


            //TODO Show dramatic dialog for this colour and quality
        }

        //Update the active state+statement index
        activeState = activeConversation.getStateByIndex(_option.target);
        activeStatement = activeState.getFirstStatement();
        activeStatementIndex = 0;
        AdvanceConversation();
    }

    //Start a state's set of statements
    private void startStatements(Conversation_State _state) {
        PLAYER_CHOOSING = false;
        activeStatement = _state.getFirstStatement();
        activeStatementIndex = 0;

        if (activeStatement.speaker == PLAYER_STRING) {
            graphics.playerStatement(activeStatement);
        } else {
            graphics.npcStatement(activeStatement);
        }
    }

    //Move onto the next statement in the current state
    private void nextStatement(Conversation_State _state) {
        PLAYER_CHOOSING = false;
        activeStatement = _state.statements[activeStatementIndex];
        activeStatementIndex += 1;

        if (activeStatement.speaker == PLAYER_STRING) {
            graphics.playerStatement(activeStatement);
        } else {
            graphics.npcStatement(activeStatement);
        }
    }

    //Hand control over to the player for choosing between a few options
    private void playerChoosing(Conversation_Transition _transition) {
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
            if (!StoryConditions.playerMeetsCondition(condition)){
                //We have found a precondition the player does not meet.
                return false;
            }
        }
        //If we have not yet returned false, then we have met all the preconditions
        return true;
    }
}
