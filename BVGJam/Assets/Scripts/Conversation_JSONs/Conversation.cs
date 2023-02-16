using UnityEngine;
using System;

[System.Serializable]
public class Conversation {

    private static string PLAYER_STRING = "Pal";

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
        Debug.Log("Conversation::getTransitionByIndex() Unknown conversation index " + _index);
        return null;
    }

    /*
        TODO Need a better way to know who we're talking with.
        Assume (for now!!!) that the NPC will speak in the first state
        Bad limitation but I don't want to change the JSON again right now.

        Want to be able to support multiple NPC speakers eventually?
        In which case every statement would have to have a left/right side
    */
    public string getNPCName() {
        foreach (Conversation_Statement statement in getFirstState().statements) {
            if (statement.speaker != PLAYER_STRING) {
                return statement.speaker;
            }
        }
        Debug.LogError("Conversation::getNPCName() Couldn't find who the player is speaking with");
        return "";
    }

    //Whether the given state is an accepting state
    public bool isAcceptingState(string _index) {
        return Array.Exists(finalStates, x => x == _index);
    }

}
