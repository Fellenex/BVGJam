using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogTrigger : MonoBehaviour {


    //A link to the dialog icon for this NPC
    public GameObject dialogIcon;

    //TODO overworld sprites+availability (remember how much time got wasted with this?)
    public Sprite dialogAvailableSprite;
    public Sprite dialogUnavailableSprite;

    private GameObject npc;                  //the parent npc with the dialog icon
    private NPC_Attributes behaviour;
    private GameObject playerReference;

    private bool triggerActive = false;

    private KeyCode dialogTriggerKey = KeyCode.E;
    
    void Start() {
        behaviour = gameObject.GetComponent<NPC_Attributes>();
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
            GetComponent<SpriteFlip>().faceLeft();
        }
        //Player is to the right of the NPC, so make the NPC face right
        else if (playerReference.transform.position.x > transform.position.x) {
            //Debug.Log("player is right of "+npc.name);
            GetComponent<SpriteFlip>().faceRight();
        }
    }

    public void checkForDialogInitiation() {
        //Don't let a player start a dialog if there's already one open
        if (!DialogManager.instance.dialogOpen && Input.GetKeyDown(dialogTriggerKey)) {
            DialogManager.instance.OnStartConversation(gameObject.name);   
        }
    }
    
}
