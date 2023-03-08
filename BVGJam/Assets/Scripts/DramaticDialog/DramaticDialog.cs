using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DramaticDialog {

    public static Dictionary<string, Color> mapTriggerToColor;

    //Store arrays of strings instead of a single string so that we can display things on multiple lines
    public static Dictionary<string, string[]> mapTriggerToTexts;


    public static Dictionary<ValueTuple<String, String>,Color> DramaticDialogColours = new System.Collections.Generic.Dictionary<ValueTuple<String, String>,Color>{
        {("red", "good"), new Color(248,25,0,255)},
        {("red", "bad"), new Color(199,120,120,255)},
        {("blue", "good"), new Color(11,3,252,255)},
        {("blue", "bad"), new Color(148,183,194,255)},
        {("yellow", "good"), new Color(255,255,51,255)},
        {("yellow", "bad"), new Color(219,224,182,255)},
        {("purple", "good"), new Color(177,0,231,255)},
        {("purple", "bad"), new Color(99,33,133,255)},
        {("green", "good"), new Color(0,232,31,255)},
        {("green", "bad"), new Color(209,237,119,255)}
    };

    public static Color getColorByTrigger(Conversation_Trigger _trigger) {
        return DramaticDialogColours[(_trigger.colour, _trigger.quality)];
    }

    //TODO
    //Read Colours.json for the dramatic dialogs
    //if no speaker/mood, don't show npc
    //if speaker/mood, show faded npc
    //write ColourConversation class
    //      has an id and a list of Conversation_Statements
    //      get colourconversation by id
}