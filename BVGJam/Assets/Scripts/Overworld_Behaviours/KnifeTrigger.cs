using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeTrigger : MonoBehaviour {
    

    public GameObject knifeAlertIcon;
    private GameObject playerReference;
    private bool triggerActive = false;
    private bool knifeFound;

    void Start() {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update() {
        if (triggerActive) {
            checkForKnifePickup();
        }
    }

    //A little alert symbol for the knife appears
    void OnTriggerEnter2D(Collider2D col) {
        if (!knifeFound && col.gameObject.name == "Player") {
            triggerActive = true;
            knifeAlertIcon.SetActive(true);
        }
    }

    //Remove the alert symbol for the knife
    void OnTriggerExit2D(Collider2D col) {
        if (!knifeFound && col.gameObject.name == "Player") {
            triggerActive = false;
            knifeAlertIcon.SetActive(false);
        }
    }

    void checkForKnifePickup() {
        if (!playerReference.GetComponent<Player>().dialogOpen
                && Input.GetKeyDown(KeyCode.E)){
    
            StoryTriggers.trigger(StoryTriggers.getFoundKnife);
            knifeAlertIcon.SetActive(false);
            knifeFound = true;

        }
    }
}
