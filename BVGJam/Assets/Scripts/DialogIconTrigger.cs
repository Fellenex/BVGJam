using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogIconTrigger : MonoBehaviour {


    //A link to the dialog icon for this NPC
    public GameObject dialogIcon;
    public Sprite dialogAvailableSprite;
    public Sprite dialogUnavailableSprite;

    private GameObject npc;                  //the parent npc with the dialog icon
    private NPC_Behaviour behaviour;
    private GameObject playerReference;

    private bool triggerActive = false;

    private string dialogSceneName = "DialogWindow";
    
    void Start() {
        npc = gameObject.transform.parent.gameObject;
        behaviour = npc.GetComponent<NPC_Behaviour>();
        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (triggerActive) {
            checkForDialogWindowPopup();
        }
    }

    //Let the player open a dialog when they are near
    //Show them a dialog icon to indicate this
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name == "Player") {
            triggerActive = true;
            dialogIcon.SetActive(true);
        }
    }

    //Remove the dialog icon that popped up
    void OnTriggerExit2D(Collider2D col) {
        triggerActive = false;
        dialogIcon.SetActive(false);
    }
    
    public void checkForDialogWindowPopup() {
        if (!playerReference.GetComponent<Player>().dialogOpen
                && Input.GetKeyDown(KeyCode.E)){

            //Only let the player open a dialog if they have not finished this conversation
            if (!StoryConditions.hasFinishedConversation(behaviour.conversationId)) {
                dialogIcon.GetComponent<SpriteRenderer>().sprite = dialogAvailableSprite;

                playerReference.GetComponent<Player>().dialogOpen = true;

                //Setup npc name and conversation ID to send over to the new scene
                Dictionary<string, string> jsonDict = new Dictionary<string,string>();

                jsonDict.Add("id", behaviour.conversationId);
                jsonDict.Add("name", behaviour.npcName);
                //jsonDict.Add("display_name", behaviour.displayName);
                DialogData.load(dialogSceneName, jsonDict);
            }
            else{
                dialogIcon.GetComponent<SpriteRenderer>().sprite = dialogUnavailableSprite;
            }
        }
    }
}
