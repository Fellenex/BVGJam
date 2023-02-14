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
    public Dictionary<string, DialogDataJSON> npcDialogFiles;
    private Dictionary<string, Conversation> npcActiveConversations;

    void Awake() {
        //Spoofing a static class while also allowing it to inherit from MonoBehaviour
        if (instance == null) { instance = this; }
        else { Destroy(this); }

        DialogController.StopConversationEvent += OnStopConversation;
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
        Debug.Log("Trying to start a conversation with " + _npcName);
        Conversation attemptedConversation = npcActiveConversations[_npcName];

        if (playerMeetsMetaconditions(attemptedConversation.metaconditions)) {
            dialogController.StartConversation(attemptedConversation);
            dialogOpen = true;
            dialogCanvas.SetActive(true);
            //Dispatch an event that the dialog prefab can pick up
            //StartConversationEvent?.Invoke(npcActiveConversations[_npcName]);
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


    //Short and sweet. Get the whole JSON for debugging
    public DialogDataJSON readWholeConversationFile(TextAsset _conversationsFile) {
        string fileContents = _conversationsFile.ToString();
        return DialogDataJSON.CreateFromJSON(fileContents);
    }

    public bool playerMeetsMetaconditions(string[] metaconditions) {
        bool conditionsMet = true;
        foreach (string condition in metaconditions) {
            if (!StoryConditions.playerMeetsCondition(condition)) {
                Debug.Log("Player has not yet met condition " + condition);
                conditionsMet = false;
            }
        }
        return conditionsMet; 
    }
}
