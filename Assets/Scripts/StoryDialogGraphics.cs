using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StoryDialogGraphics : DialogGraphics {

    //Apparently we have to set Colors with normalized (0-1) values if it's editing an Image from a script
    //Problem: https://issuetracker.unity3d.com/issues/image-color-cannot-be-changed-via-script-when-image-type-is-set-to-simple
    //Solution: https://forum.unity.com/threads/ui-image-color-not-updating-correctly.674827/
    private static Dictionary<ValueTuple<String, String>,Color> TRIGGER_COLOURS =
        new System.Collections.Generic.Dictionary<ValueTuple<String, String>,Color>{
        {("red", "good"),       new Color(248f/255f,    25f/255f,   0f,         1f)},
        {("red", "bad"),        new Color(199f/255f,    120f/255f,  120f/255f,  1f)},
        {("blue", "good"),      new Color(11f/255f,     3f/255f,    252f/255f,  1f)},
        {("blue", "bad"),       new Color(148f/255f,    183f/255f,  194f/255f,  1f)},
        {("yellow", "good"),    new Color(255f/255f,    255f/255f,  51f/255f,   1f)},
        {("yellow", "bad"),     new Color(219f/255f,    224f/255f,  182f/255f,  1f)},
        {("purple", "good"),    new Color(177f/255f,    0f,         231f/255f,  1f)},
        {("purple", "bad"),     new Color(99f/255f,     33f/255f,   133f/255f,  1f)},
        {("green", "good"),     new Color(0f,           232f/255f,  31f/255f,   1f)},
        {("green", "bad"),      new Color(209f/255f,    237f/255f,  119f/255f,  1f)}
    };

    private static Color getColorByTrigger(Conversation_Trigger _trigger) {
        return TRIGGER_COLOURS[(_trigger.colour, _trigger.quality)];
    }

    public override void HandleTrigger(Conversation_Trigger _trigger) {
        resetTextElements();

        //Update the background's colour to reflect the current colour trigger
        //TODO future - have something more dramatic. Fade-in, not just the default background image, etc
        childPanel.GetComponent<Image>().color = getColorByTrigger(_trigger);
        Debug.Log(childPanel.GetComponent<Image>().color);
    }
}
