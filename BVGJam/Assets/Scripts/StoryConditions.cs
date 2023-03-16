using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StoryConditions {

    public static String GOOD_QUALITY = "good";
    public static String BAD_QUALITY = "bad";

    public static List<String> COLOURS = new List<String> { "blue", "red", "green", "purple", "yellow" };

    public static int goodColoursCount = 0;
    public static int badColoursCount = 0;

    public static List<String> specialConditions = new List<String>{
        "foundKnife",               //Make the knife disappear when we "pick it up"
        "clericTransformation"      //Change the Cleric's name from "Casey_The_Cleric" to "Casey_The_Heretic"
        };


    public static void HandleTrigger(Conversation_Trigger _trigger) {
        //Keep track that the player has met this specific colour
        //Also keep track of the number of good+bad colours they have acquired
        ConditionManager.MeetCondition(_trigger.colour);
        if (_trigger.quality == GOOD_QUALITY) {
            goodColoursCount++;
        } else if (_trigger.quality == BAD_QUALITY) {
            badColoursCount++;
        } else {
            Debug.LogError("StoryConditions::HandleTrigger() incorrect quality (" + _trigger.quality + ")");
        }

        if (isSpecialCondition(_trigger.text)) {

            if (_trigger.text == "foundKnife") {
                GameObject.Find("Knife").SetActive(false);
            }

            if (_trigger.text == "clericTransformation") {
                GameObject.Find("Saint_Casey").name = "Casey_The_Heretic";
            }
        }

    }

    public static bool isSpecialCondition(String _condition) {
        return specialConditions.Contains(_condition);
    }
    
}
