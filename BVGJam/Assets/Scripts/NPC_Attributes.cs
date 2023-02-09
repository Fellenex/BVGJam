using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Attributes : MonoBehaviour {
    public string npcName = "FillerName";
    public string displayName = "Filler Name";

    public string classType;

    public Sprite dialogIconPleased;
    public Sprite dialogIconNeutral;
    public Sprite dialogIconUpset;


    //The pipeline from TextAsset (file) to DialogDataJSON (serialized data) to current conversation
    public TextAsset conversationJSON;
}
