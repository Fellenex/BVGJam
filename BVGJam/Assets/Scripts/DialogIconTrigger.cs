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
    
    public void checkForDialogWindowPopup() {
        if (!playerReference.GetComponent<Player>().dialogOpen
                && Input.GetKeyDown(KeyCode.E)){

            //Only let the player open a dialog if they have not finished this conversation
            if (!StoryConditions.hasFinishedConversation(behaviour.conversationId)) {
                dialogIcon.GetComponent<SpriteRenderer>().sprite = dialogAvailableSprite;

                playerReference.GetComponent<Player>().dialogOpen = true;

                //Setup npc name and conversation ID to send over to the new scene
                Dictionary<string, string> jsonDict = new Dictionary<string,string>();

                //TODO find the conversation id based on in-game data, rather than assuming
                jsonDict.Add("id", behaviour.conversationId);
                jsonDict.Add("name", behaviour.npcName);
                jsonDict.Add("display_name", behaviour.displayName);
                DialogData.load("DialogWindow", jsonDict);
            }
            else{
                dialogIcon.GetComponent<SpriteRenderer>().sprite = dialogUnavailableSprite;
                    
            }
        }
    }


    //TODO very very messy. Absolutely refuses to cooperate and offset like 10 pixels above the NPC
    void OnTriggerEnter2D(Collider2D col) {

        if (col.gameObject.name == "Player") {
            triggerActive = true;

            //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);

            //Get the offset position for the dialog icon, instantiate it, and set its position
            //Vector3 dPos = new Vector3(transform.position.x, transform.position.y + verticalOffset, 0);

            //Debug.Log("Spawned at " + dPos + " , " + transform.position);

            dialogIcon.SetActive(true);
            //dialogIconReference = Object.Instantiate(prefabDialogIcon, transform.parent.transform.position, Quaternion.identity);
            ////dialogIconReference.transform.parent = gameObject.parent.GetComponentInParent<Transform>();
            //dialogIconReference.transform.parent = transform.parent;


            //dialogIconReference.transform.Translate(new Vector3 (0, offset, 0));
            
            

            //Adding this puts the dialog icon as a child of the trigger object instead of the NPC
            //dialogIconReference.transform.parent = gameObject.transform;
            
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        triggerActive = false;
        dialogIcon.SetActive(false);
        //Object.Destroy(dialogIconReference);
    }
}
