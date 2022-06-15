using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_JSONS : MonoBehaviour {

    public TextAsset jsonToTest;

    void Start() {
        debugConversationJSON(jsonToTest);
    }

    //Short and sweet. Get the whole JSON for debugging
    public DialogDataJSON readWholeConversationFile(TextAsset _conversationsFile) {

        string fileContents = _conversationsFile.ToString();
        DialogDataJSON convos = DialogDataJSON.CreateFromJSON(fileContents);
        return convos;
    }

    //Check some common mistakes with our JSON files
    public void debugConversationJSON(TextAsset _conversationsFile) {

        DialogDataJSON json = readWholeConversationFile(_conversationsFile);

        List<string> stateLabels = new List<string>();
        List<string> transitionLabels = new List<string>();

        List<string> metaconditions = new List<string>();
        List<string> conditions = new List<string>();
        List<string> triggers = new List<string>();

        //Start by trying to find any undefined entries (debug.log will receive null and get upset)
        foreach (Conversation convo in json.conversations) {
            Debug.Log(convo.id);
            Debug.Log(convo.starter);

            Debug.Log(convo.finalStates);
            foreach (string fs in convo.finalStates) {
                Debug.Log(fs);
            }

            Debug.Log(convo.metaconditions);
            foreach (string mc in convo.metaconditions) {
                Debug.Log(mc);

                if (!metaconditions.Contains(mc)) {
                    metaconditions.Add(mc);
                }
            }

            Debug.Log(convo.states);
            foreach (Conversation_NPCState state in convo.states) {
                Debug.Log(state.index);

                //keep track of the state indices for alter
                if (!stateLabels.Contains(state.index)) {
                    stateLabels.Add(state.index);
                }

                Debug.Log(state.speaker);
                if (state.speaker.Contains(" ")) {
                    Debug.LogError("Convo: "+convo.id+" has a space in the speaker's name ("+state.speaker+")");
                }

                //Make sure each statement has text and mood, and that the mood is an allowable string
                Debug.Log(state.statements);
                foreach (Conversation_Statement ment in state.statements) {
                    Debug.Log(ment.text);
                    Debug.Log(ment.mood);

                    if (ment.mood != "pleased" && ment.mood != "upset" && ment.mood != "neutral") {
                        Debug.LogError("Convo: "+convo.id+" has incorrect mood labeling ("+ment.mood+")");
                    }
                }
            }

            Debug.Log(convo.transitions);
            foreach (Conversation_Transition tran in convo.transitions) {
                Debug.Log(tran.source);
                Debug.Log(tran.target);

                //Keep track of the source/target state labels
                if (!transitionLabels.Contains(tran.source)) {
                    transitionLabels.Add(tran.source);
                }
                if (!transitionLabels.Contains(tran.target)) {
                    transitionLabels.Add(tran.target);
                }

                //Make sure each statement has text and mood, and that the mood is an allowable string
                Debug.Log(tran.statements);
                foreach (Conversation_Statement ment in tran.statements) {
                    Debug.Log(ment.text);
                    Debug.Log(ment.mood);

                    if (ment.mood != "pleased" && ment.mood != "upset" && ment.mood != "neutral") {
                        Debug.LogError("Convo: "+convo.id+" has Incorrect mood labeling");
                    }
                }
                Debug.Log(tran.optionText);
                Debug.Log(tran.conditions);
                foreach (string cond in tran.conditions) {
                    Debug.Log(cond);

                    //Keep track of conditions to compare them against our global list
                    if (!conditions.Contains(cond)) {
                        conditions.Add(cond);
                    }
                }
                Debug.Log(tran.triggers);
                foreach (string trig in tran.triggers) {
                    Debug.Log(trig);

                    //Keep track of triggers to compare them against our global list
                    if (!triggers.Contains(trig)) {
                        triggers.Add(trig);
                    }
                }
            }
        }

        //Test to see if the NPC state labels and the player transition labels are the same set
        foreach (string label in stateLabels) {
            if (!transitionLabels.Contains(label)) {
                Debug.LogError("State label " + label + " does not appear in any transition");
            }
        }
        foreach (string label in transitionLabels) {
            if (!stateLabels.Contains(label)) {
                Debug.LogError("Transition label "+ label + " does not appear in any state");
            }
        }

        //Test to see if the triggers are all ones that we know about
        foreach (string trigger in triggers) {
            if (!StoryTriggers.triggers.Contains(trigger)) {
                
                string choppedString = trigger.Substring(1, trigger.Length-1);
                //Check for inverted triggers (starting with a !)
                if (!StoryTriggers.triggers.Contains(choppedString)) {
                    Debug.LogError("Trigger " + trigger + " is not a known trigger ("+choppedString+")");
                }
            }
        }

        //Test to see if the conditions are all ones that we know about
        foreach (string condition in conditions) {
            string triggerNameFromCondition = condition.Replace(StoryTriggers.conditionPrefix, StoryTriggers.triggerPrefix);

            //Check for inverted conditions (starting with a !)
            if (!StoryTriggers.triggers.Contains(triggerNameFromCondition)) {

                string choppedString = triggerNameFromCondition.Substring(1, triggerNameFromCondition.Length-1);
                if (!StoryTriggers.triggers.Contains(choppedString)) {
                    Debug.LogError("Condition " + condition + " is not a known condition ("+choppedString+")");
                }
            }
        }

        //Test to see if the metaconditions are all ones that we know about
        foreach (string condition in metaconditions) {
            string triggerNameFromCondition = condition.Replace(StoryTriggers.conditionPrefix, StoryTriggers.triggerPrefix);

            if (!StoryTriggers.triggers.Contains(triggerNameFromCondition)) {
                
                string choppedString = triggerNameFromCondition.Substring(1, triggerNameFromCondition.Length-1);
                //Check for inverted conditions (starting with a !)
                if (!StoryTriggers.triggers.Contains(choppedString)) {
                    Debug.LogError("Tried to find "+condition.Replace(StoryTriggers.conditionPrefix, StoryTriggers.triggerPrefix));
                    Debug.LogError("Metacondition " + condition + " is not a known condition ("+choppedString+")");
                }
            }
        }
    }
}
