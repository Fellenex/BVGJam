using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogTrigger : MonoBehaviour {


    //A link to the dialog icon for this NPC
    public GameObject dialogIcon;
    public Sprite dialogAvailableSprite;
    public Sprite dialogUnavailableSprite;

    private GameObject npc;                  //the parent npc with the dialog icon
    private NPC_Attributes behaviour;
    private GameObject playerReference;

    private bool triggerActive = false;

    private KeyCode dialogTriggerKey = KeyCode.E;

    DialogDataJSON json;
    
    void Start() {
        npc = gameObject.transform.parent.gameObject;
        behaviour = npc.GetComponent<NPC_Attributes>();
        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (triggerActive) {
            checkForDialogInitiation();
        }
    }

    //Let the player open a dialog when they are near
    //Show them a dialog icon to indicate this
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name == playerReference.name) {
            triggerActive = true;
            //dialogIcon.SetActive(true);
            //Look towards the player as they arrive
            lookTowardsPlayer();
        }
    }

    //Remove the dialog icon that popped up
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name == playerReference.name) {
            triggerActive = false;
            //dialogIcon.SetActive(false);
            //Look towards the player as they leave
            lookTowardsPlayer();
        }
    }
    
    void lookTowardsPlayer() {
        //Player is to the left of the NPC, so make the NPC face left
        if (playerReference.transform.position.x < transform.position.x) {
            //Debug.Log("player is left of "+npc.name);
            npc.GetComponent<SpriteFlip>().faceLeft();
        }
        //Player is to the right of the NPC, so make the NPC face right
        else if (playerReference.transform.position.x > transform.position.x) {
            //Debug.Log("player is right of "+npc.name);
            npc.GetComponent<SpriteFlip>().faceRight();
        }
    }

    public void checkForDialogInitiation() {

        if (!DialogManager.instance.dialogOpen && Input.GetKeyDown(dialogTriggerKey)) {

            Debug.Log("Player has tried to start a conversation");

            //TODO all of this logic should be in DialogManager.
            //DialogTrigger should only be responsible for telling DialogManager to try and open the conversation

            //Only let the player open a dialog if they have not finished this conversation
            Debug.Log(behaviour.classType);
            Debug.Log(StoryConditions.nextConversationIdByActor);

            DialogManager.instance.dialogOpen = true;
            DialogManager.instance.OnStartConversation(npc.name);

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
        }
    }

    public bool playerMeetsMetaconditions(string[] metaconditions) {
        bool conditionsMet = true;
        foreach (string cond in metaconditions) {
            if (!StoryConditions.playerMeetsCondition(cond)) {
                conditionsMet = false;
            }
        }
        return conditionsMet; 
    }
    
}
