using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    DialogDataJSON json;
    
    void Start() {
        npc = gameObject.transform.parent.gameObject;
        behaviour = npc.GetComponent<NPC_Behaviour>();
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
        if (col.gameObject.name == "Player") {
            triggerActive = true;
            dialogIcon.SetActive(true);
            //Look towards the player as they arrive
            lookTowardsPlayer();
        }
    }

    //Remove the dialog icon that popped up
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name == "Player") {
            triggerActive = false;
            dialogIcon.SetActive(false);
            //Look towards the player as they leave
            lookTowardsPlayer();
        }
    }
    
    void lookTowardsPlayer() {
        //Player is to the left of the NPC, so make the NPC face left
        if (playerReference.transform.position.x < transform.position.x) {
            Debug.Log("player is left of "+npc.name);
            npc.GetComponent<SpriteFlip>().faceLeft();
        }
        //Player is to the right of the NPC, so make the NPC face right
        else if (playerReference.transform.position.x > transform.position.x) {
            Debug.Log("player is right of "+npc.name);
            npc.GetComponent<SpriteFlip>().faceRight();
        }
    }

    public void checkForDialogInitiation() {
        if (!playerReference.GetComponent<Player>().dialogOpen
                && Input.GetKeyDown(KeyCode.E)){

            //Only let the player open a dialog if they have not finished this conversation
            if (!StoryConditions.hasFinishedConversation(behaviour.conversationId)) {
                dialogIcon.GetComponent<SpriteRenderer>().sprite = dialogAvailableSprite;

                playerReference.GetComponent<Player>().dialogOpen = true;

                //Setup npc name and conversation ID to send over to the new scene
                Dictionary<string, string> dData = new Dictionary<string,string>();

                //dData.Add("id", behaviour.conversationID);
                dData.Add("name", behaviour.npcName);
                //jsonDict.Add("display_name", behaviour.displayName);
                DialogData.setActiveConversation(advancedReadFile(behaviour.conversationJson, behaviour.conversationId));
                DialogData.load(dialogSceneName, dData);
            }
            else{
                dialogIcon.GetComponent<SpriteRenderer>().sprite = dialogUnavailableSprite;
            }
        }
    }

    public Conversation advancedReadFile(TextAsset _conversationsFile, string _id) {
        Debug.Log("trying to find " + _conversationsFile.name);
        Conversation acCo = null;

        string fileContents = _conversationsFile.ToString();
        Debug.Log("Found the following: ");
        Debug.Log(fileContents);
        json = DialogDataJSON.CreateFromJSON(fileContents);

        Debug.Log("looking for id "+_id);
        //Loop through the conversations to try to find a matching id
        foreach (Conversation con in json.conversations) {
            if (_id == con.id){
                acCo = con;
                break;
            }
        }
        if (acCo == null) {
            Debug.Log("No conversation(x) with id " + _id + " found");
        }
        return acCo;
    }
}
