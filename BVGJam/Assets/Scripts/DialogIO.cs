using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogIO : MonoBehaviour {

    public TextAsset jsonToTest;

    void Start() {
        validateDialogFile(readWholeConversationFile(jsonToTest));
    }

    //Short and sweet. Get the whole JSON for debugging
    public DialogData readWholeConversationFile(TextAsset _conversationsFile) {
        string fileContents = _conversationsFile.ToString();
        DialogData convos = DialogData.CreateFromJSON(fileContents);
        return convos;
    }

    //Check some common mistakes with our JSON files
    public void validateDialogFile(DialogData dialog){ 
        foreach (Conversation conversation in dialog.conversations) {
            conversation.validate();

            //Do some cross-validation between transition labels and state labels
            foreach (Conversation_Transition transition in conversation.transitions) {
                //Make sure all of our transitions start at real states
                noisyAssertion(Array.Exists(conversation.states, state => state.index == transition.source),
                    "Transition source "+transition.source+" can't be found among the states in conversation "+conversation.id);

                //Make sure all of our transitions end at real states
                foreach (Conversation_Option option in transition.options) {
                    noisyAssertion(Array.Exists(conversation.states, state => state.index == option.target),
                        "Transition destination "+option.target+" can't be found among the states in conversation "+conversation.id);
                }
            }
        }
    }

    public void validateAllDialogFiles() {
        List<DialogData> dialogs = new List<DialogData>();
        List<String> triggers = new List<String>();
        List<String> conditions = new List<String>();

        //Collect all of the trigger and condition text labels from the dialog files
        foreach (DialogData dialog in dialogs) {
            foreach (Conversation conversation in dialog.conversations) {
                triggers.AddRange(conversation.getTriggerLabels());
                conditions.AddRange(conversation.getConditionLabels());
            }
        }

        //Each trigger should also be, at some point, required as a condition
        foreach (String trigger in triggers) {
            Debug.Assert(conditions.Exists(condition => condition == trigger));
        }

        //Each condition should also be, at some point, meetable by a trigger
        foreach (String condition in conditions) {
            Debug.Assert(triggers.Exists(trigger => trigger == condition));
        }
    }


    //If condition isn't met, then we debug the message
    public void noisyAssertion(bool condition, string message) {
        if (!condition) {
            Debug.LogError(message);
        }
    }
}
