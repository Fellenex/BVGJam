using System;
using UnityEngine;

[System.Serializable]
public class Conversation_Trigger {
    public string text = "";
    public string quality;
    public string colour;

    //If a trigger has colour+quality, then it's a special "event trigger"
    //In this case, we need to know because we'll show them a "dramatic dailog"
    public bool isSpecialTrigger() {
        return (!String.IsNullOrEmpty(quality) && !String.IsNullOrEmpty(colour));
    }


    public void validate() {
        //If we have a trigger, then the text can't be empty
        //Debug.Assert(!String.IsNullOrEmpty(text));
        //TODO this seems to be firing when the list of triggers is empty

        //If we have a quality attribute, then we need to have a colour attribute
        //(Not necessarily the other way around)
        if (!string.IsNullOrEmpty(quality)) {
            Debug.Assert(!String.IsNullOrEmpty(colour));
        }
    }
}
