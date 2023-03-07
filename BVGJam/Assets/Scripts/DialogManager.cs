using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
//using UnityEngine.SceneManagement;


public class DialogManager : MonoBehaviour {

    public static DialogManager instance;

    public bool dialogOpen = false;

    //These should all be set in the Unity Editor
    public DialogController dialogController;
    public GameObject audioController;       // a handle to change audio volume based on conversation status


    //A list of all of the NPC gameobjects
    public List<GameObject> npcs;
    public List<DialogData> dialogFiles;

    private Dictionary<string, List<Conversation>> npcConversations;

    void Awake() {
        //Spoofing a static class while also allowing it to inherit from MonoBehaviour
        if (instance == null) { instance = this; }
        else { Destroy(this); }

        //When the controller tells us the conversation is over, take back control
        DialogController.StopConversationEvent += OnStopConversation;
    }

   void Start() {
        dialogFiles = new List<DialogData>();
        npcConversations = new Dictionary<string, List<Conversation>>();

        //Read all of the conversation files for NPCs managed by DialogManager
        foreach (GameObject npc in npcs) {

            //TODO for now, skip over inactive NPCs (avoid noisy JSON debugging)            
            if (npc.activeSelf) {
                TextAsset dialogFile = npc.GetComponent<NPC_Attributes>().dialogFile;
                DialogData dialog = DialogIO.ReadDialogFile(dialogFile);

                //Validate the conversations in this dialog file
                dialog.validate();

                npcConversations[npc.name] = new List<Conversation>(dialog.conversations);
            }
        }

        //Validate the conditions+triggers across all NPCs
        crossCheckConditionsAndTriggers(dialogFiles);
    }

    public void OnStartConversation(string _npcName) {
        Debug.Log("Trying to start a conversation with " + _npcName);

        //Start a conversation if it's unambiguous which one should come next
        Conversation nextConversation = getNextConversation(_npcName);
        if (nextConversation != null) {
            Debug.Log("About to start conversation '" + nextConversation.id + "' with '" + _npcName + "'");
            audioController.GetComponent<AudioSource>().volume = 0.5f;
            dialogController.StartConversation(nextConversation);
            dialogOpen = true;
        }
    }

    public void OnStopConversation(Conversation _previousConversation){
        Debug.Log("Ending the active conversation");

        dialogOpen = false;
        audioController.GetComponent<AudioSource>().volume = 1.0f;

        //StopConversationEvent?.Invoke();
        /*
        Receive a list of triggers that should get flipped as a result of this conversation ending
        Receive a list of the state label that the conversation was stopped in

        Trigger relevant items
        Compare supplied state label against final states set for this Conversation object

        */
    }


    //Gets the next conversation for a given NPC name.
    //If there's more than one available, reports an error
    private Conversation getNextConversation(string _npcName) {
        List<Conversation> possibleConversations = getPossibleConversations(_npcName);
        switch(possibleConversations.Count) {
            case 0:
                return null;
            case 1:
                return possibleConversations[0];
            default:
                Debug.LogError("DialogManager::getNextConversation() Too many Conversations available");
                foreach (Conversation c in possibleConversations) {
                    Debug.LogError(String.Join(", ", c.id));    
                }
                return null;
        }
    }

    //Gets a list of all the possible conversations the NPC might have
    private List<Conversation> getPossibleConversations(string _npcName) {
        List<Conversation> possibleConversations = new List<Conversation>();

        foreach ((String,String) x in ConditionManager.conversationStatus.Keys) {
            Debug.Log(x + " --> " + ConditionManager.conversationStatus[x]);
        }

        //Check the metaconditions for each one, and make sure we haven't
        //  already finished the conversation
        foreach (Conversation conversation in npcConversations[_npcName]){
            if (hasMetMetaconditions(conversation.metaconditions)){
                if (!ConditionManager.hasFinishedConversation(_npcName, conversation.id)) {
                    Debug.Log("Player has not yet finished conversation " + conversation.id);
                    possibleConversations.Add(conversation);
                } else {
                    Debug.Log("Player already finished conversation " + conversation.id);
                }
            } else {
                //Debug.Log("Player does not meet the conditions for " + conversation.id);
            }
        }
        return possibleConversations;
    }

    //Returns boolean corresponding to whether or not the player has already met a list of metaconditions
    private bool hasMetMetaconditions(string[] metaconditions) {
        bool conditionsMet = true;
        foreach (String condition in metaconditions) {
            if (!ConditionManager.hasMetCondition(condition)) {
                //Debug.Log("Player has not yet met metacondition " + condition);
                conditionsMet = false;
            }
        }
        return conditionsMet; 
    }

    /*
    Compares all of the dialog files to make sure that
        1. Every condition required to enter a conversation has some transition which triggers it
        2. Every condition required to take a transition has some transition which triggers it
        3. Every trigger found on a transition has some condition which requires it
    */
    private static void crossCheckConditionsAndTriggers(List<DialogData> _dialogs) {
        List<String> triggers = new List<String>();
        List<String> conditions = new List<String>();

        //Collect all of the trigger and condition text labels from the dialog files
        foreach (DialogData dialog in _dialogs) {
            foreach (Conversation conversation in dialog.conversations) {
                triggers.AddRange(conversation.getTriggerLabels());
                conditions.AddRange(conversation.getConditionLabels());
            }
        }

        //Each trigger should also be, at some point, required as a condition
        foreach (String trigger in triggers) {
            if (!conditions.Exists(condition => condition == trigger)) {
                Debug.LogError("Trigger " + trigger + " is not found on any condition");
            }
        }

        //Each condition should also be, at some point, meetable by a trigger
        foreach (String condition in conditions) {
            if (!triggers.Exists(trigger => trigger == condition)) {
                Debug.LogError("Condition " + condition + "is not found on any trigger");
            }
        }
    }
}
