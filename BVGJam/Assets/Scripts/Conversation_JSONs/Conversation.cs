using UnityEngine;

[System.Serializable]
public class Conversation {

    public string id;

    public string[] finalStates;

    public string[] metaconditions;

    public Conversation_State[] states;

    public Conversation_Transition[] transitions;

    //Gets the first state in a conversation.
    public Conversation_State getFirstState() {
        return states[0];
    }

    public Conversation_State getStateByIndex(string _index) {
        foreach (Conversation_State state in states) {
            if (state.index == _index) {
                //Found it!
                return state;
            }
        }
        Debug.LogError("Conversation::getStateByIndex() Unknown state index " + _index);
        return null;
    }

    //Find the transition information for a given state's index
    public Conversation_Transition getTransitionByIndex(string _index) {
        foreach (Conversation_Transition transition in transitions) {
            if (transition.source == _index) {
                //Found it!
                return transition;
            }
        }
        Debug.LogError("Conversation::getTransitionByIndex() Unknown conversaiton index " + _index);
        return null;
    }
}
