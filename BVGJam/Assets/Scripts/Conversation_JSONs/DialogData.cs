using System;
using UnityEngine;

[System.Serializable]
public class DialogData {
    
    public Conversation[] conversations;

    //Check some common mistakes with our JSON files
    //Initiates the recursive calls to validate() on all child Conversation_X objects
    //X \in {State, Statement, Transition, Option, Trigger}
    public void validate() {
        foreach (Conversation conversation in conversations) {
            conversation.validate();

            //Do some cross-validation between transition labels and state labels
            foreach (Conversation_Transition transition in conversation.transitions) {
                //Make sure all of our transitions start at real states
                if (!Array.Exists(conversation.states, state => state.index == transition.source)) {
                    Debug.LogError("Transition source " + transition.source + " can't be found among the states in conversation " + conversation.id);
                }

                //Make sure all of our transitions end at real states
                foreach (Conversation_Option option in transition.options) {
                    if (!Array.Exists(conversation.states, state => state.index == option.target)) {
                        Debug.LogError("Transition destination "+option.target+" can't be found among the states in conversation "+conversation.id);
                    }   
                }
            }
        }
    }
}
