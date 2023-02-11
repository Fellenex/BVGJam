using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Attributes : MonoBehaviour {

    public string classType;


    //The pipeline from TextAsset (file) to DialogDataJSON (serialized data) to current conversation
    public TextAsset conversationJSON;
}
