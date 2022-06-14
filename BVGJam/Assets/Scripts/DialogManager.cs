using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour, IDialogMessages {

    //Speaker.NONE happens when the player is choosing their dialog option
    enum Speaker {PLAYER, NPC, NONE}
    Speaker activeSpeaker;

    public DialogGraphics graphics;        // a handle to update the graphics

    DialogDataJSON json;
    Conversation activeConversation;
    Conversation_NPCState activeState;
    Conversation_Transition activeTransition;
    Conversation_Statement activeStatement;
    int activeStatementIndex;

    /*
    //How many seconds to wait for the player to read each character of text
    public float secondsPerCharacterToWait = 5f;
    IEnumerator StatementWait() {
        yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.E));
    }
    */

    void Start() {
        graphics = GetComponent<DialogGraphics>();

        string npcName = DialogData.getParam("name");
        string jsonFilename = "Assets/Dialog/" + npcName + ".json";

        //Load the .json file with this NPC's conversations
        activeConversation = readFile(jsonFilename);
        StoryConditions.startConversation(npcName, activeConversation.id);

        activeState = activeConversation.states[0]; 
        activeTransition = null;
        Debug.Log("Final states");
        foreach ( var x in activeConversation.finalStates){
            Debug.Log(x.ToString());
        }

        if (activeConversation.starter == "player") {
            //If the player is starting the conversation and they only have
            //  one opening line, then they just say it right away
            List<Conversation_Transition> initialOptions = getPossibleTransitions();
            if (initialOptions.Count == 1){
                activeTransition = initialOptions[0];
                playerSpeaking(initialOptions[0]);
            }
            //If the player has no initial options then don't start the conversation
            else if (initialOptions.Count == 0){
                Debug.Log("Conversation "+DialogData.getParam("id")+" had no options. Exiting before it starts");
                closeConversation();
            }
            //If the player has more than one choice, then let them choose!
            else {
                playerChoosing(getPossibleTransitions());
            }
        }
        else{
            npcSpeaking(activeState);
        }
    }

    void Update() {
        //Only let the player advance the state if they are not currently choosing
        if (activeSpeaker != Speaker.NONE
            && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
            advanceConversation();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            closeConversation();
        }
    }

    /*
    I think we don't want to bother setting activeTransition/activeState to null
        throughout the functions playerChoosing, playerSpeaking, 
        playerContinuesSpeaking, npcSpeaking, npcContinuesSpeaking       
    */

    //Hand control over to the player for choosing between a few options
    public void playerChoosing(List<Conversation_Transition> _playerTransitions) {
        activeSpeaker = Speaker.NONE;
        graphics.playerIsChoosing(_playerTransitions);
    }

    //Hand control over to the player for speech
    //Sets constants and fires a request off to the graphics script to display things
    public void playerSpeaking(Conversation_Transition _transition) {
        activeSpeaker = Speaker.PLAYER;
        activeTransition = _transition;
        activeStatementIndex = 0;
        activeStatement = _transition.statements[0];

        if (_transition.triggers.Length > 0) {
            //By choosing this transition, we have triggered some code change
            foreach(string triggerText in _transition.triggers){
                StoryTriggers.trigger(triggerText);
            }
        }

        graphics.playerStatement(activeStatement);
    }
    //Continuing a player's set of statements in the current transition
    public void playerContinuesSpeaking(Conversation_Transition _transition) {
        //Speaker and transition don't change, but the statement will
        activeStatementIndex += 1;
        activeStatement = _transition.statements[activeStatementIndex];

        graphics.playerStatement(activeStatement);
    }


    //Hand control over to the NPC for speech.
    //Sets constants and fires a request off to the graphics script to display things
    public void npcSpeaking(Conversation_NPCState _npcState) {
        activeSpeaker = Speaker.NPC;
        activeState = _npcState;
        activeStatementIndex = 0;
        activeStatement = _npcState.statements[activeStatementIndex];

        graphics.npcStatement(activeStatement, turnCodeNameIntoDisplayName(_npcState.speaker));
    }
    //Continuing an NPC's set of statements in the current state
    public void npcContinuesSpeaking(Conversation_NPCState _npcState){
        //Speaker and state don't change, but the statement will
        activeStatementIndex += 1;
        activeStatement = _npcState.statements[activeStatementIndex];

        graphics.npcStatement(activeStatement, turnCodeNameIntoDisplayName(_npcState.speaker));
    }

    //Helper functions to check whether the supplied statement is the last statement of either:
    //  an NPC's current state, or a player's current transition
    //  (both of which have lists of Conversation_Statement objects)
    public bool hasMoreStatements(Conversation_NPCState _npcState, Conversation_Statement _npcStatement) {
        return _npcStatement != _npcState.statements[_npcState.statements.Length - 1];
    }
    public bool hasMoreStatements(Conversation_Transition _playerTransition, Conversation_Statement _playerStatement) {
        return _playerStatement != _playerTransition.statements[_playerTransition.statements.Length - 1];
    }

    //TODO timers to auto-advance based on how many characters the text is?
    public void advanceConversation(){

        //Player was speaking their line when the trigger to advance happened
        if (activeSpeaker == Speaker.PLAYER) {
        
            if (hasMoreStatements(activeTransition, activeStatement)) {
                //The player isn't finished speaking the current set of dialog windows
                playerContinuesSpeaking(activeTransition);
            }
            else{
                //Move forward to the next NPC speaking state
                try {
                    npcSpeaking(getNewNPCState(activeTransition));
                }
                catch (Exception e){
                    Debug.Log("No more states available from NPC");
                    closeConversation();
                }
            }
        }

        //NPC was speaking their line when the trigger to advance happened
        else if (activeSpeaker == Speaker.NPC) {

            if (hasMoreStatements(activeState, activeStatement)) {
                //The NPC isn't finished speaking the current set of dialog windows
                npcContinuesSpeaking(activeState);
            }
            else {
                //Swap from NPC speaking back to player dialog options
                List<Conversation_Transition> currTransitions = getPossibleTransitions();
                if (currTransitions.Count > 0) {
                    //Force the transition if there's only one choice unless it
                    //  has a precondition or a trigger (a "special" transition)
                    if ((currTransitions.Count == 1)
                            && currTransitions[0].conditions.Length == 0
                            && currTransitions[0].triggers.Length == 0) {
                        playerSpeaking(currTransitions[0]);
                    }
                    else {
                        playerChoosing(currTransitions);
                    }
                }
                else{
                    //The player has advanced and they have no more options, so close things up
                    closeConversation();
                }
            }
        }
    }

    public void closeConversation() {
        Debug.Log("Checking to see if "+activeState.index+" is a final state");
        foreach ( var x in activeConversation.finalStates){
            Debug.Log(x.ToString());
        }
        //If we are in a final state for the NPC, then we mark this conversation as completed
        
        foreach (int state in activeConversation.finalStates) {
            if (activeState.index == state) {
                Debug.Log("Completed conversation "+activeState.index);
                StoryConditions.finishConversation(activeConversation.id);
            }
        }
        GetComponent<CloseDialogWindow>().dialogClose();
    }

    public Conversation readFile(string _filename) {
        Debug.Log("trying to find " + _filename);
        activeConversation = null;

        if (File.Exists(_filename)){
            string fileContents = File.ReadAllText(_filename);
            json = DialogDataJSON.CreateFromJSON(fileContents);

            //Loop through the conversations to try to find a matching id
            foreach (Conversation con in json.conversations) {
                if (DialogData.getParam("id") == con.id){
                    activeConversation = con;
                    break;
                }
            }
            if (activeConversation == null) {
                Debug.LogError("No conversation with id " + DialogData.getParam("id") + " found");
            }
        }
        else{
            Debug.Log("file doesn't exist");
        }
        return activeConversation;
    }


    //Look through all the transitions to see if any match the currently active state
    //If any do, then also check whether the player meets the preconditions necessary for it
    public List<Conversation_Transition> getPossibleTransitions() {
        List<Conversation_Transition> possibleTransitions = new List<Conversation_Transition>();
        for (var i=0; i < activeConversation.transitions.Length; i++) {
            if (activeConversation.transitions[i].source == activeState.index
                    && playerMeetsPreconditions(activeConversation.transitions[i])){
                
                possibleTransitions.Add(activeConversation.transitions[i]);
                Debug.Log("Adding the following possible transition: " + activeConversation.transitions[i].statements[0].text);
            }
        }

        return possibleTransitions;
    }


    //Gets the NPC state that will result from taking _transition
    public Conversation_NPCState getNewNPCState(Conversation_Transition _transition) {
        return activeConversation.states[_transition.target];
    }

    //Checks to see if player meets the preconditions for a given transition
    public bool playerMeetsPreconditions(Conversation_Transition _transition) {
        Debug.Log("Condition(s) Required: " + _transition.conditions);
        foreach (string condition in _transition.conditions) {
            if (!StoryConditions.doesPlayerMeetCondition(condition)){
                //We have found a precondition the player does not meet.
                return false;
            }
        }
        //If we have not yet returned false, then we have met all the preconditions
        return true;
    }

    //User has selected a text option! We should display it
    public void buttonClicked(Button button) {
        playerSpeaking(button.GetComponent<TransitionHolder>().transition);
    }

    public string turnCodeNameIntoDisplayName(string _codeName) {
        return _codeName.Replace("_", " ");
    }
}
