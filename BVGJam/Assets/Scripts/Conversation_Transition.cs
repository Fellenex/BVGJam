using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation_Transition {
    public string source;
    public string target;
    public Conversation_Statement[] statements;
    public string optionText;
    public string[] conditions;
    public string[] triggers;
}
