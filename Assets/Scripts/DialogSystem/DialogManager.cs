using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Linq;
//using UnityEngine.SceneManagement;


public class DialogManager : MonoBehaviour {

    public static DialogManager instance;

    public bool dialogOpen = false;

    //These should all be set in the Unity Editor
    public DialogController dialogController;
    public GameObject audioController;       // a handle to change audio volume based on conversation status

    public StoryConditionManager conditionManager;

    //A list of all of the NPC gameobjects
    public List<GameObject> npcs;
    public List<List<Conversation>> dialogFiles;

    private Dictionary<String, List<Conversation>> npcConversations;

    public enum ConversationStatus { Uninitiated, Started, Finished };
    public static Dictionary<String, ConversationStatus> conversationStatus = new Dictionary<String, ConversationStatus>();

    void Awake() {
        //Spoofing a static class while also allowing it to inherit from MonoBehaviour
        if (instance == null) { instance = this; }
        else { Destroy(this); }

        //When the controller tells us the conversation is over, take back control
        DialogController.StopConversationEvent += OnStopConversation;
    }

   void Start() {
        npcConversations = new Dictionary<String, List<Conversation>>();
        Debug.Log("Dialog Manager start");

        //Read all of the conversation files for NPCs managed by DialogManager
        foreach (GameObject npc in npcs) {
            //Skip over inactive NPCs. They may not have been set up yet
            if (npc.activeSelf) {
                TextAsset dialogFile = npc.GetComponent<NPC_Attributes>().dialogFile;

                if (dialogFile == null){
                    Debug.LogWarning(npc.name + " missing a conversation file. Did you mean to do this?");
                } else {
                    List<Conversation> conversations = DialogIO.ReadDialogFile(dialogFile);
                    //Validate the conversations in this dialog file
                    foreach (Conversation conversation in conversations){
                        conversation.validate();
                        conversationStatus[conversation.id] = ConversationStatus.Uninitiated;
                    }

                    npcConversations[npc.name] = conversations;
                }

            }
        }

        //Validate the conditions+triggers across all NPCs
        crossCheckConditionsAndTriggers(npcConversations);
    }

    public void OnStartConversation(String _npcName) {
        Debug.Log("Trying to start a conversation with " + _npcName);

        //Start a conversation if it's unambiguous which one should come next
        Conversation nextConversation = getNextConversation(_npcName);
        if (nextConversation != null) {
            Debug.Log("About to start conversation '" + nextConversation.id + "' with '" + _npcName + "'");
            audioController.GetComponent<AudioSource>().volume = 0.5f;
            conversationStatus[nextConversation.id] = ConversationStatus.Started;
            dialogController.StartConversation(nextConversation);
            dialogOpen = true;
        }
    }

    public void OnStopConversation(Conversation _previousConversation, bool _isFinalState){
        Debug.Log("Ending the active conversation");

        if (_isFinalState) {
            //Keep track of having finished this conversation
            Debug.Log("Final state. Finishing conversation '" + _previousConversation.id);
            conversationStatus[_previousConversation.id] = ConversationStatus.Finished;
        }

        dialogOpen = false;
        audioController.GetComponent<AudioSource>().volume = 1.0f;
    }


    //Gets the next conversation for a given NPC name.
    //If there's more than one available, reports an error
    private Conversation getNextConversation(String _npcName) {
        List<Conversation> possibleConversations = getPossibleConversations(_npcName);
        switch(possibleConversations.Count) {
            case 0:
                Debug.Log("DialogManager::getNextConversation() Didn't find any available conversations");
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
    private List<Conversation> getPossibleConversations(String _npcName) {
        List<Conversation> possibleConversations = new List<Conversation>();

        conditionManager.prettyPrintConditions();

        //Check the metaconditions for each one, and make sure we haven't
        //  already finished the conversation
        foreach (Conversation conversation in npcConversations[_npcName]){
            if (hasMetMetaconditions(conversation.metaconditions)){
                if (!hasFinishedConversation(conversation.id)) {
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
    private bool hasMetMetaconditions(String[] metaconditions) {
        bool conditionsMet = true;
        foreach (String condition in metaconditions) {
            if (!conditionManager.hasMetCondition(condition)) {
                //Debug.Log("Player has not yet met metacondition " + condition);
                conditionsMet = false;
            }
        }
        return conditionsMet; 
    }

    public bool hasFinishedConversation(String _conversationId) {
        try {
            return conversationStatus[_conversationId] == ConversationStatus.Finished;
        } catch (KeyNotFoundException) {
            conversationStatus[_conversationId] = ConversationStatus.Uninitiated;
            return false;
        }
    }

    /*
    Compares all of the conversations to make sure that
        1. Every condition required to enter a conversation has some transition which triggers it
        2. Every condition required to take a transition has some transition which triggers it
        3. Every trigger found on a transition has some condition which requires it
    */
    private static void crossCheckConditionsAndTriggers(Dictionary<String, List<Conversation>> npcConversations) {
        List<String> triggers = new List<String>();
        List<String> conditions = new List<String>();
        List<String> conversation_ids = new List<String>();

        //Collect all of the trigger and condition text labels from the dialog files
        foreach (List<Conversation> conversations in npcConversations.Values) {
            foreach (Conversation conversation in conversations) {
                triggers.AddRange(conversation.getTriggerLabels());
                conditions.AddRange(conversation.getConditionLabels());
                conversation_ids.Add(conversation.id);
            }
        }

        //Each trigger should also be, at some point, required as a condition
        foreach (String trigger in triggers) {
            if (!conditions.Exists(condition => condition == trigger)) {
                Debug.Log("Trigger " + trigger + " is not found on any condition");
            }
        }

        //Each condition should also be, at some point, meetable by a trigger
        foreach (String condition in conditions) {
            if (!triggers.Exists(trigger => trigger == condition)) {
                Debug.Log("Condition " + condition + " is not found on any trigger");
            }
        }

        //Each conversation id should be unique
        if (conversation_ids.Count != conversation_ids.Distinct().Count()) {
            Debug.Log("There exist duplicate conversation ids");
            foreach (String conversation_id in conversation_ids) {
                Debug.Log(conversation_id);
            }
        }
    }
}
