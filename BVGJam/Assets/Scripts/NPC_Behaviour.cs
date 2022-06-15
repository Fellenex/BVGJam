using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Behaviour : MonoBehaviour {
    public string npcName = "FillerName";
    public string displayName = "Filler Name";
    public string conversationId = "opener";

    public Sprite dialogIconPleased;
    public Sprite dialogIconNeutral;
    public Sprite dialogIconUpset;

    public TextAsset conversationJson;

    void Start() {
    }
}
