using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogIconTrigger : MonoBehaviour {

    public GameObject prefabDialogIcon;         //the icon to create
    private GameObject npc;                  //the parent npc with the dialog icon

    //Keep track of the dialog icon so we can get rid of it when the player leaves the trigger
    private GameObject dialogIconReference;
    private GameObject playerReference;

    private bool dialogPossible = false;
    private int verticalOffset;
    
    void Start() {
        npc = transform.parent.gameObject;
        NPC_Behaviour behaviour = npc.GetComponent<NPC_Behaviour>();
        verticalOffset = behaviour.dialogIconOffsetHeight;

        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (dialogPossible) {
            checkForDialogWindowPopup();
        }
    }
    
    public void checkForDialogWindowPopup() {
        if (!playerReference.GetComponent<Player>().dialogOpen
                && Input.GetKeyDown(KeyCode.E)){

            playerReference.GetComponent<Player>().dialogOpen = true;

            //Setup npc name and conversation ID to send over to the new scene
            Dictionary<string, string> jsonDict = new Dictionary<string,string>();

            //TODO find the conversation id based on in-game data, rather than assuming
            jsonDict.Add("id", "opener");
            
            jsonDict.Add("name", npc.GetComponent<NPC_Behaviour>().npcName);
            DialogData.load("DialogWindow", jsonDict);
        }
    }


    //TODO very very messy. Absolutely refuses to cooperate and offset like 10 pixels above the NPC
    void OnTriggerEnter2D(Collider2D col) {

        if (col.gameObject.name == "Player") {
            
            dialogPossible = true;

            //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);

            //Get the offset position for the dialog icon, instantiate it, and set its position
            Vector3 dPos = new Vector3(transform.position.x, transform.position.y + verticalOffset, 0);

            //Debug.Log("Spawned at " + dPos + " , " + transform.position);

            dialogIconReference = Object.Instantiate(prefabDialogIcon, transform.parent.transform.position, Quaternion.identity);
            //dialogIconReference.transform.parent = gameObject.parent.GetComponentInParent<Transform>();
            dialogIconReference.transform.parent = transform.parent;


            //dialogIconReference.transform.Translate(new Vector3 (0, offset, 0));
            
            

            //Adding this puts the dialog icon as a child of the trigger object instead of the NPC
            //dialogIconReference.transform.parent = gameObject.transform;
            
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        dialogPossible = false;
        Object.Destroy(dialogIconReference);
    }
}
