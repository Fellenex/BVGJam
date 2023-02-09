using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class DialogManager : MonoBehaviour {

    public static DialogManager instance;


    public GameObject audioController;       // a handle to change audio volume based on conversation status
    public GameObject dialogCanvas;         // a handle to change the relevant conversation shown
    public GameObject environment;          // a handle to disable/enable the environment

    public event Action<Conversation> StartConversationEvent;
    public event Action<Conversation> StopConversationEvent;
    public event Action AdvanceConversation;


    //map npc names to their respective dialog files
    public List<GameObject> npcs;
    public Dictionary<string, DialogDataJSON> npcDialogFiles;
    private Dictionary<string, Conversation> npcActiveConversations;

    private string dialogSceneName = "DialogWindow";

    void Awake() {
        //Spoofing a static class while also allowing it to inherit from MonoBehaviour
        if (instance == null) { instance = this; }
        else { Destroy(this); }

        Debug.Log("Dialog Manager has awoken");
    }

   void Start() {
        npcDialogFiles = new Dictionary<string, DialogDataJSON>();
        npcActiveConversations = new Dictionary<string, Conversation>();

        //Read all of the conversation files for NPCs managed by DialogManager
        foreach (GameObject npc in npcs) {

            npcDialogFiles[npc.name] = readWholeConversationFile(npc.GetComponent<NPC_Attributes>().conversationJSON);
            
            //Assume the first conversation is the starting one if we don't have one defined otherwise
            //TODO allow for custom starting conversation IDs to be specified
            npcActiveConversations[npc.name] = npcDialogFiles[npc.name].conversations[0];
        }
    }

    public void OnStartConversation(string _npcName) {
        Debug.Log("Started a conversation with " + _npcName);

        //environment.SetActive(false);
        dialogCanvas.SetActive(true);

        //Dispatch an event that the dialog prefab can pick up
        StartConversationEvent?.Invoke(npcActiveConversations[_npcName]);
    }

    //void Start() {
        /*
        graphics = GetComponent<DialogGraphics>();

        //Load up the data setup through the DialogData class
        npcName = DialogData.getParam("name");
        npcClass = DialogData.getParam("class");
        activeConversation = DialogData.getActiveConversation();
        try {
            //Setup things needed for starting a conversation
            StoryConditions.startConversation(npcName, activeConversation.id);
            activeState = activeConversation.states[0]; 
            activeTransition = null;
        }
        catch (NullReferenceException e) {
            //Break out of the start function early to avoid more errors
            //  due to an unavailable file
            closeConversation();
        }

        //Reduce the volume of music while in a conversation
        audioController.GetComponent<AudioSource>().volume = 0.5f;

        //We always start with a statement, whether it's from the player or the NPC
        if (activeState.statements[0].speaker == "player") {

            beginStateSpeech(activeState);

            //playerSpeaking(activeState);

            /*

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

            */
        //}
        //else{
        //    npcSpeaking(activeState);
        //}
    //}





    public void OnStopConversation(Conversation _conversation) {
        Debug.Log("Ended a conversation " + _conversation.id);

        /*
        Receive a list of triggers that should get flipped as a result of this conversation ending
        Receive a list of the state label that the conversation was stopped in

        Trigger relevant items
        Compare supplied state label against final states set for this Conversation object

        */

        //environment.SetActive(true);
        dialogCanvas.SetActive(false);

        StopConversationEvent?.Invoke(_conversation);
    }

    //"Gracefully" close out the conversation.
    public void closeConversation() {
        //Put the music back to normal
        audioController.GetComponent<AudioSource>().volume = 1.0f;



        //TODO fix this up
        /*
        try {
            //If we are in a final state for the NPC, then we mark this conversation as completed
            foreach (string state in activeConversation.finalStates) {
                if (activeState.index == state) {
                    Debug.Log("Completed conversation "+activeState.index);
                    StoryConditions.finishConversation(activeConversation.id, npcClass);

                    //get a handle to the npc to change their conversation id
                }
            }
        }
        catch (NullReferenceException e) {
            //If we are closing out of the conversation because there was some issue
            //  obtaining the conversation, then it won't have any finalStates property.
        }
        GetComponent<CloseDialogWindow>().dialogClose();
        */
    }


    //Short and sweet. Get the whole JSON for debugging
    public DialogDataJSON readWholeConversationFile(TextAsset _conversationsFile) {
        string fileContents = _conversationsFile.ToString();
        return DialogDataJSON.CreateFromJSON(fileContents);
    }
}
