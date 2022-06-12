using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation_Transition {
    public int source;
    public int target;
    public string text;
    public string mood;
    public StoryCondition[] conditions;
    public StoryTrigger[] triggers;
}
