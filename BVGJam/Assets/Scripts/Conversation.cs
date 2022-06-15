using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation {

    public string id;

    public string starter;

    public string[] finalStates;

    public string[] metaconditions;

    public Conversation_NPCState[] states;

    public Conversation_Transition[] transitions;
}
