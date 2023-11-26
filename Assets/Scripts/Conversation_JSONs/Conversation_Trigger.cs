using System;
using UnityEngine;

[System.Serializable]
public class Conversation_Trigger {
    public string text = "";
    public string quality;
    public string colour;

    public void validate() {
        Debug.Assert(!String.IsNullOrEmpty(text));
    }
}
