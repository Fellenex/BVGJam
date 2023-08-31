using System;
using UnityEngine;

[System.Serializable]
public class Conversation_Option {
    public string target;
    public string optionText;
    public string[] conditions;
    public Conversation_Trigger[] triggers;

    public void validate() {
        Debug.Assert(!String.IsNullOrEmpty(target));

        foreach (Conversation_Trigger trigger in triggers) {
            trigger.validate();
        }
    }
}
