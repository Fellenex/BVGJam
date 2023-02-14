using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {

    public static string PLAYER_STRING = "Pal";

    bool PLAYER_CHOOSING = false;

    public DialogGraphics graphics;        // a handle to update the graphics


    Conversation activeConversation;
    Conversation_State activeState;
    Conversation_Transition activeTransition;
    Conversation_Statement activeStatement;
    int activeStatementIndex;

    public static event Action<string> StopConversationEvent;

    void Awake() {

        Debug.Log("Adding event handlers for Start/Stop conversation");

        //Add the StartConversation and StopConversation functions as handlers
        //  for when we receive an event from the DialogManager
        //DialogManager.instance.StartConversationEvent += StartConversation;
        //DialogManager.instance.StopConversationEvent += StopConversation;


    }
    public void Start() {
        DialogGraphics.instance.SetNextStateEvent += ChooseOption;
    }

    void Update() {
        //Only let the player advance the state if they are not currently choosing
        if (!PLAYER_CHOOSING && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
            AdvanceConversation();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            DialogManager.instance.OnStopConversation(activeState.index);
        }
    }

    public void StartConversation(Conversation _conversation) {
        Debug.Log("DialogController::StartConversation() has begun ("+_conversation.id+")");

        activeConversation = _conversation;
        activeState = _conversation.getFirstState();

        //TODO Need a better way to know who we're talking with.
        //Assume (for now!!!) that the NPC will speak in the first state
        //Bad limitation but I don't want to change the JSON again right now
        string activeNPC = "";
        foreach (Conversation_Statement statement in activeState.statements) {
            if (statement.speaker != PLAYER_STRING) {
                activeNPC = statement.speaker;
                break;
            }
        }
        if (activeNPC == "") {
            Debug.LogError("DialogController::StartConversation() Couldn't find who the player is speaking with");
        }

        //Initialize the finished status of this conversation
        StoryConditions.startConversation(activeNPC, _conversation.id);
        graphics.initializeConversation(activeNPC);
        startStatements(activeState);
    }

    public void StopConversation(){
        Debug.Log("DialogController::StopConversation() has begun");

        if (activeConversation.isAcceptingState(activeState.index)) {
            //We've finished this conversation.
            StoryConditions.finishConversation(activeConversation.id, "Paladin");
        }

        //Here we should fire an event to tell the dialog manager that the conversation can be closed
        StopConversationEvent?.Invoke(activeState.index);
    }

    //TODO timers to auto-advance based on how many characters the text is?
    private void AdvanceConversation(){
        if (activeState.hasMoreStatements(activeStatementIndex)) {
            nextStatement(activeState);
        } else {
            //No more statements - see if we have a transition that we can take to another state
            Conversation_Transition tr = activeConversation.getTransitionByIndex(activeState.index);
            if (tr != null) {
                //Then we show the player the transition options
                //TODO Should only show transition options that the player has met the conditions for
                playerChoosing(tr);
            } else {
                //No more statements available, and no transitions available
                Debug.Log("Stopping conversation - no more statements available");
                StopConversation();
            }
        }
    }


    //The player has chosen a speech option - update the conversation to reflect this choice
    private void ChooseOption(Conversation_Option _option) {
        //If there were triggers, then cause them here
        foreach (string trigger in _option.triggers) {
            StoryConditions.playerHasMetCondition(trigger);
        }

        //Update the active state
        activeState = activeConversation.getStateByIndex(_option.target);
        AdvanceConversation();
    }

    //Start with this set of statements
    private void startStatements(Conversation_State _state) {
        PLAYER_CHOOSING = false;
        activeState = _state;   //redundant but maybe good to keep in
        activeStatement = _state.getFirstStatement();
        activeStatementIndex = 0;

        if (activeStatement.speaker == PLAYER_STRING) {
            graphics.playerStatement(activeStatement);
        } else {
            graphics.npcStatement(activeStatement);
        }
    }

    //Move onto the next statement
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
    private void playerChoosing(Conversation_Transition _playerOptions) {
        PLAYER_CHOOSING = true;
        graphics.playerIsChoosing(_playerOptions);
    }


    /*
    //Look through all the transitions to see if any match the currently active state
    //If any do, then also check whether the player meets the preconditions necessary for it
    public Conversation_Transition getPossibleTransitions() {
        //Find which transition leads out of the current state
        Conversation_Transition possibleTransitions = new List<Conversation_Transition>();

        /*
        for (var i=0; i < activeConversation.transitions.Length; i++) {

            //Debug.Log("Comparing the two strings: '"+activeConversation.transitions[i].source.ToString()+"' and '"+activeState.index.ToString()+"'");
            //Debug.Log("The result is: ");
            //Debug.Log(activeConversation.transitions[i].source.ToString() == activeState.index.ToString());
            //Debug.Log("9" == activeState.index.ToString());
            //Debug.Log("9" == activeConversation.transitions[i].source.ToString());

            //Debug.Log("Comparing the two strings: '"+activeConversation.transitions[i].source+"' and '"+activeState.index+"'");
            //Debug.Log("The result is: ");
            //Debug.Log(activeConversation.transitions[i].source.ToString().Equals(activeState.index));

            if (activeConversation.transitions[i].source.ToString() == activeState.index.ToString()
                    && playerMeetsPreconditions(activeConversation.transitions[i])){
                
                possibleTransitions.Add(activeConversation.transitions[i]);
                Debug.Log("Adding the following possible transition: " + activeConversation.transitions[i].statements[0].text);
            }
        }
        */
        /*
        return possibleTransitions;
    }
    */

    


    //Gets the NPC state that will result from taking _transition
    public Conversation_State getNewNPCState(Conversation_Transition _transition) {
        return null;
        /*
        TODO was not referenced when I commented it out
        foreach (Conversation_State state in activeConversation.states) {
            if (state.index.ToString() == _transition.target.ToString()) {
                return state;
            }
        }
        return null; //activeConversation.states[_transition.target];
        */
    }

    //Checks to see if player meets the conditions for a given transition option
    public bool playerMeetsConditions(Conversation_Option _transitionOption) {

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

    /*
    //User has selected a text option! We should display it
    public void buttonClicked(Button button) {
        //TODO not just option 0
        var targetState = button.GetComponent<OptionHolder>().option.target;
        playerSpeaking(button.GetComponent<OptionHolder>().transition);
    }
    */


    /*
    //How many seconds to wait for the player to read each character of text
    public float secondsPerCharacterToWait = 5f;
    IEnumerator StatementWait() {
        yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.E));
    }
    */
}
