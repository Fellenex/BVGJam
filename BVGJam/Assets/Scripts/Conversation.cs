using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation {

    public string id;

    public string starter;

    public Conversation_NPCState[] states;

    public Conversation_Transition[] transitions;
}
