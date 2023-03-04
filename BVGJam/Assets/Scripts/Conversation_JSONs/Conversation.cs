using UnityEngine;
using System;
using System.Collections.Generic;

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
        Debug.LogError("Conversation::getStateByIndex() Unknown state index " + _index
                + " in conversation " + id);
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
        Debug.Log("Conversation::getTransitionByIndex() No transitions with source " + _index
                + " in conversation " + id);
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
        Debug.LogError("Conversation::getNPCName() Couldn't find who the player is speaking with "
                + " in conversation " + id);
        return "";
    }

    //Whether or not the given state is an accepting state
    public bool isAcceptingState(string _index) {
        return Array.Exists(finalStates, x => x == _index);
    }

    public List<String> getTriggerLabels() {
        List<String> triggers = new List<String>();

        foreach (Conversation_Transition transition in transitions) {
            foreach (Conversation_Option option in transition.options) {
                foreach (Conversation_Trigger trigger in option.triggers) {
                    triggers.Add(trigger.text);
                }
            }
        }
        return triggers;
    }

    public List<String> getConditionLabels() {
        List<String> conditions = new List<String>();

        //We may have conditions preceding a conversation
        foreach (String metacondition in metaconditions) {
            conditions.Add(metacondition);
        }
        
        //Otherwise, the rest of the conditions appear as part of a transition's option
        foreach (Conversation_Transition transition in transitions) {
            foreach (Conversation_Option option in transition.options) {
                foreach (String condition in option.conditions) {
                    conditions.Add(condition);
                }
            }
        }
        
        return conditions;
    }

    public void validate() {
        Debug.Log("Starting validation on " + id);
        
        //We need a name for this conversation for tracking conversation completeness
        Debug.Assert(!String.IsNullOrEmpty(id));

        //We need at least one final state to know when we can consider the conversation "complete"
        Debug.Assert(finalStates.Length > 0);

        //We need at least one state to be able to deliver any statements
        Debug.Assert(states.Length > 0);

        //If we have more than one state, then we should have at least one transition
        if (states.Length > 1) {
            Debug.Assert(transitions.Length > 0);
        }

        foreach (Conversation_State state in states) {
            state.validate();
        }

        foreach (Conversation_Transition transition in transitions) {
            transition.validate();
        }
    }
}
