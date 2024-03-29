using System;
using UnityEngine;

[System.Serializable]
public class Conversation_Trigger {
    public string text = "";
    public string quality;
    public string colour;

    private static string GOOD_QUALITY = "good";
    private static string BAD_QUALITY = "bad";
    private static string NONE_QUALITY = "none";

    //If a trigger has colour+quality, then it's a special "event trigger"
    //In this case, we need to know because we'll show them a "dramatic dailog"
    public bool isColourTrigger() {
        return (!String.IsNullOrEmpty(quality) && !String.IsNullOrEmpty(colour));
    }

    public bool isGoodQualityTrigger() { return quality == GOOD_QUALITY; }
    public bool isBadQualityTrigger() { return quality == BAD_QUALITY; }
    public bool isNoneQualityTrigger() { return quality == NONE_QUALITY; }


    public void validate() {
        //If we have a trigger, then the text can't be empty
        Debug.Assert(!String.IsNullOrEmpty(text));

        //If we have a quality attribute, then we need to have a colour attribute
        //(Not necessarily the other way around)
        if (!string.IsNullOrEmpty(quality)) {
            Debug.Assert(!String.IsNullOrEmpty(colour));

            Debug.Assert(isGoodQualityTrigger() || isBadQualityTrigger() || isNoneQualityTrigger());
        }
    }
}
