using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Behaviour : MonoBehaviour {
    public string npcName = "FillerName";
    public string displayName = "Filler Name";
    public string conversationId = "opener";

    public TextAsset conversationJson;

    void Start() {
        //rigidbody = GetComponent<Rigidbody2D>();

        if (npcName == "Grandma_Yaga") {
            displayName = "Grandma Yaga";
        }
        else{
            displayName = npcName;
        }
    }
}
