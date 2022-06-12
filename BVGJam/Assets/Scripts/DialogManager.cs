using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour, IDialogMessages {

    //Speaker.NONE happens when the player is choosing their dialog option
    enum Speaker {PLAYER, NPC, NONE}
    Speaker activeSpeaker;

    public DialogGraphics graphics;        // a handle to update the graphics

    bool isNPCFinished = false;

    DialogDataJSON json;
    Conversation activeConversation;
    Conversation_NPCState activeState;
    Conversation_Transition activeTransition;

    void Start() {
        graphics = GetComponent<DialogGraphics>();

        string npcName = DialogData.getParam("name");
        string jsonFilename = "Assets/Dialog/" + npcName + ".json";

        //Load the .json file with this NPC's conversations
        activeConversation = readFile(jsonFilename);
        activeState = activeConversation.states[0];
        activeTransition = null;

        if (activeConversation.starter == "player") {
            //If the player is starting the conversation and they only have
            //  one opening line, then they just say it right away
            List<Conversation_Transition> initialOptions = getPossibleTransitions();
            if (initialOptions.Count == 1){
                activeTransition = initialOptions[0];
                playerSpeaking(initialOptions[0]);
            }
            else if (initialOptions.Count == 0){
                //TODO do we need to check for something here for "Gracefully ended" conversations?
                Debug.Log("What to do here?");
            }
            else {
                //If the player has more than one choice, then let them choose!
                playerChoosing(getPossibleTransitions());
            }
            
        }
        else{
            //TODO / NOTE : Assumes the first NPC state is the conversation starter
            //  Also therefore assumes the first NPC state's mood is the starting mood
            npcSpeaking(activeState);
        }

        //Debug.Log("Started a dialog: " + npcName + " and " + PLAYER_NAME + " are having a(n) "+activeMood+" conversation" +activeSpeakerName+" started it");

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
        Consider whether we should be e.g., setting activeTransition = null in playerChoosing or npcSpeaking
        similarly for activeState
    */

    public void playerChoosing(List<Conversation_Transition> _playerTransitions) {
        activeSpeaker = Speaker.NONE;
        graphics.playerIsChoosing(_playerTransitions);
    }

    public void playerSpeaking(Conversation_Transition _chosenTransition) {
        activeSpeaker = Speaker.PLAYER;
        activeTransition = _chosenTransition;

        if (_chosenTransition.triggers.Length > 0) {
            //By choosing this transition, we have triggered some code change
            foreach(string triggerText in _chosenTransition.triggers){
                StoryTriggers.trigger(triggerText);
            }
        }
        graphics.playerIsSpeaking(_chosenTransition);
    }

    public void npcSpeaking(Conversation_NPCState _npcState) {
        activeSpeaker = Speaker.NPC;
        activeState = _npcState;
        graphics.npcIsSpeaking(_npcState);
    }

    //TODO timers to auto-advance based on how many characters the text is?
    public void advanceConversation(){

        //Player was speaking their line when the trigger to advance happened
        //Move forward to the next NPC speaking state
        if (activeSpeaker == Speaker.PLAYER) {
            npcSpeaking(getNewNPCState(activeTransition));
        }

        //Swap from NPC speaking back to player dialog options
        else if (activeSpeaker == Speaker.NPC) {
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

    public void closeConversation() {
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

    public List<Conversation_Transition> getPossibleTransitions() {
        List<Conversation_Transition> possibleTransitions = new List<Conversation_Transition>();

        //Look through all the transitions to see if any match the currently active state
        //If any do, then also check whether the player meets the preconditions necessary for it
        for (var i=0; i < activeConversation.transitions.Length; i++) {
            if (activeConversation.transitions[i].source == activeState.index
                    && playerMeetsPreconditions(activeConversation.transitions[i])){
                
                possibleTransitions.Add(activeConversation.transitions[i]);
                Debug.Log("Adding the following possible transition: " + activeConversation.transitions[i].text);
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
}
