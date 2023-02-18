using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEngine.SceneManagement;


public class DialogManager : MonoBehaviour {

    public static DialogManager instance;

    public bool dialogOpen = false;

    public DialogController dialogController;
    public GameObject audioController;       // a handle to change audio volume based on conversation status
    public GameObject dialogCanvas;         // a handle to change the relevant conversation shown

    //public event Action<Conversation> StartConversationEvent;
    //public event Action AdvanceConversation;

    //map npc names to their respective dialog files
    public List<GameObject> npcs;
    public Dictionary<string, DialogData> npcDialogFiles;
    private Dictionary<string, Conversation> npcActiveConversations;

    private new Dictionary<string, List<Conversation>> npcConversations;

    void Awake() {
        //Spoofing a static class while also allowing it to inherit from MonoBehaviour
        if (instance == null) { instance = this; }
        else { Destroy(this); }

        DialogController.StopConversationEvent += OnStopConversation;
    }

   void Start() {
        npcDialogFiles = new Dictionary<string, DialogData>();
        npcConversations = new Dictionary<string, List<Conversation>>();

        //Read all of the conversation files for NPCs managed by DialogManager
        foreach (GameObject npc in npcs) {

            npcDialogFiles[npc.name] = readWholeConversationFile(npc.GetComponent<NPC_Attributes>().conversationJSON);
            npcConversations[npc.name] = new List<Conversation>(npcDialogFiles[npc.name].conversations);

            //Assume the first conversation is the starting one if we don't have one defined otherwise
            //TODO allow for custom starting conversation IDs to be specified
            //npcActiveConversations[npc.name] = npcDialogFiles[npc.name].conversations[0];
        }
    }

    public void OnStartConversation(string _npcName) {
        Debug.Log("Trying to start a conversation with " + _npcName);

        //Start a conversation if it's unambiguous which one should come next
        Conversation nextConversation = getNextConversation(_npcName);
        if (nextConversation != null) {
            Debug.Log("About to start conversation (" + nextConversation.id + ")");
            audioController.GetComponent<AudioSource>().volume = 0.5f;
            dialogController.StartConversation(nextConversation);
            dialogOpen = true;
            dialogCanvas.SetActive(true);
        }
    }

    public void OnStopConversation(string _activeStateIndex){
        Debug.Log("Ending the active conversation");

        dialogOpen = false;
        dialogCanvas.SetActive(false);
        audioController.GetComponent<AudioSource>().volume = 1.0f;

        //TODO
        //Setup what the next conversation for that NPC should be.
        //Probably we should pass the conversation itself over, but we also need to know whether that conversation got finished
        //Currently DialogController keeps track of whether the conversation was completed
        //Here, then, we should just be deciding which conversation comes next.
        //If the previous conversation was finished, then setup for the next one. 

        /*
            if (!StoryConditions.hasFinishedConversation(StoryConditions.nextConversationIdByActor[behaviour.classType])) {

                

                //Setup npc name and conversation ID to send over to the new scene
                Dictionary<string, string> dData = new Dictionary<string,string>();

                //dData.Add("id", behaviour.conversationID);
                dData.Add("name", behaviour.npcName);
                dData.Add("class", behaviour.classType);
                //jsonDict.Add("display_name", behaviour.displayName);
                
                Debug.Log("Opening up "+behaviour.conversationJson);
                Debug.Log("Looking for "+behaviour.activeConversation.id);


                //DialogData.setActiveConversation(advancedReadFile(behaviour.conversationJson, behaviour.conversationId));

                foreach (Conversation x in behaviour.jsonFile.conversations) {

                    Debug.Log("checking to see if player meets metaconditions "+x.metaconditions);
                    if (x.id == StoryConditions.nextConversationIdByActor[behaviour.classType]
                            && playerMeetsMetaconditions(x.metaconditions)) {
                        

                        Debug.Log("They did meet the metaconditions!");
                        DialogData.setActiveConversation(x);
                    }
                }

                DialogData.load(dialogSceneName, dData);

                
            }
            else{
                //dialogIcon.GetComponent<SpriteRenderer>().sprite = dialogUnavailableSprite;
            }

        */

        //StopConversationEvent?.Invoke();
        /*
        Receive a list of triggers that should get flipped as a result of this conversation ending
        Receive a list of the state label that the conversation was stopped in

        Trigger relevant items
        Compare supplied state label against final states set for this Conversation object

        */

        //environment.SetActive(true);
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

        //Check the metaconditions for each one, and make sure we haven't
        //  already finished the conversation
        foreach (Conversation conversation in npcConversations[_npcName]){
            if (playerMeetsMetaconditions(conversation.metaconditions) &&
                !StoryConditions.hasFinishedConversation(conversation.id)) {
                    possibleConversations.Add(conversation);
            }
        }
        return possibleConversations;
    }

    //Short and sweet. Get the whole JSON for debugging
    private DialogData readWholeConversationFile(TextAsset _conversationsFile) {
        string fileContents = _conversationsFile.ToString();
        return DialogData.CreateFromJSON(fileContents);
    }

    private bool playerMeetsMetaconditions(string[] metaconditions) {
        bool conditionsMet = true;
        foreach (String condition in metaconditions) {
            if (!StoryConditions.playerMeetsCondition(condition)) {
                Debug.Log("Player has not yet met metacondition " + condition);
                conditionsMet = false;
            }
        }
        return conditionsMet; 
    }
}
