using System;
using UnityEngine;

[System.Serializable]
public class Conversation_Transition {
    public string source;
    public Conversation_Option[] options;

    public void validate() {
        //Every transition needs a source state
        Debug.Assert(!String.IsNullOrEmpty(source));

        //Each transition should have at least one option
        Debug.Assert(options.Length > 0);

        foreach (Conversation_Option option in options) {
            option.validate();
        }
    }
}
