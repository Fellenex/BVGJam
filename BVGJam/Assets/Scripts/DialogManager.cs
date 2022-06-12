using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour, IDialogMessages {

    static string PLAYER_NAME = "Couleur";

    enum Speaker {Player, NPC, NONE}
    Speaker activeSpeaker;
    string activeSpeakerName;
    string activeMood;

    bool isNPCFinished = false;

    Conversation activeConversation;

    public DialogGraphics graphics;        // a handle to update the graphics

    DialogDataJSON json;

    int activeStateIndex = 0;
    Conversation_NPCState activeState;

    void Start() {
        graphics = GetComponent<DialogGraphics>();

        string npcName = DialogData.getParam("name");
        string jsonFilename = "Assets/Dialog/" + npcName + ".json";

        //Load the .json file with this NPC's conversations
        activeConversation = readFile(jsonFilename);
        activeState = activeConversation.states[0];

        if (activeConversation.starter == "player") {
            Debug.Log("Player is starting conversation");
            playerSpeaking(getPossibleTransitions());
        }
        else{
            //TODO / NOTE : Assumes the first NPC state is the conversation starter
            //  Also therefore assumes the first NPC state's mood is the starting mood
            Debug.Log(npcName + " is starting conversation");
            npcSpeaking(activeState);
        }

        Debug.Log("Started a dialog: " + npcName + " and " + PLAYER_NAME + " are having a(n) "+activeMood+" conversation" +activeSpeakerName+" started it");

    }

    void Update() {
        //Only let the player advance the state if the NPC is currently speaking
        if (activeSpeaker == Speaker.NPC
            && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))) {
            advanceConversation();
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            closeConversation();
        }
    }

    public void playerChoosing() {
        //Split up functionality between speaking and choosing

    }

    public void playerSpeaking(List<Conversation_Transition> _playerTransitions) {
        activeSpeaker = Speaker.Player;
        activeSpeakerName = PLAYER_NAME;

        //TODO Assumes the first player transition listed is the mood-setting one
        //NOTE: Kinda janky with different moods for multiple starting positions
        activeMood = _playerTransitions[0].mood;

        graphics.playerIsSpeaking(_playerTransitions, activeMood);
    }

    public void npcSpeaking(Conversation_NPCState _npcState) {
        activeSpeaker = Speaker.NPC;

        //TODO do we actually need activeSpeakerName here in DialogManager?
        activeSpeakerName = _npcState.speaker;
        activeState = _npcState;

        graphics.npcIsSpeaking(_npcState.speaker, _npcState.text, _npcState.mood);
    }

    public void advanceConversation(){
        //Swap from NPC speaking back to player dialog options
        List<Conversation_Transition> currTransitions = getPossibleTransitions();
        if (currTransitions.Count > 0) {
            playerSpeaking(currTransitions);
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

    public Conversation_NPCState getNewNPCState(Conversation_Transition _transition) {
        return activeConversation.states[_transition.target];
    }


    public bool playerMeetsPreconditions(Conversation_Transition _transition) {
        Debug.Log("Condition Required: " + _transition.conditions);
        return true;
    }

    //User has selected a text option!
    public void buttonClicked(Button button) {
        Debug.Log("Clicked :" + button.GetComponentInChildren<Text>().text);

        //Find the transition object associated with the button that was just clicked
        //Then, determine which NPC state should be entered as a result of the transition
        //Then, hand control back over to the NPC to display their text
        npcSpeaking(getNewNPCState(button.GetComponent<TransitionHolder>().transition));
    }
}
