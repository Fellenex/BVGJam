using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryConditionManager : ConditionManager {

    private static String GOOD_QUALITY = "good";
    private static String BAD_QUALITY = "bad";

    private static List<String> COLOURS = new List<String> { "blue", "red", "green", "purple", "yellow" };

    private int goodColoursCount = 0;
    private int badColoursCount = 0;

    private List<String> specialConditions = new List<String>{
        "foundKnife",               //Make the knife disappear when we "pick it up"
        "clericTransformation"      //Change the Cleric's name from "Casey_The_Cleric" to "Casey_The_Heretic"
        };

    /*
    Returns the special trigger of the list
    */
    public void HandleTriggers(Conversation_Trigger[] _triggers) {
        foreach (Conversation_Trigger trigger in _triggers) {
            HandleTrigger(trigger);
        }
    }

    /*
    Returns true if it was a special trigger
    */
    public override void HandleTrigger(Conversation_Trigger _trigger) {
        Debug.Log("StoryConditionManager::HandleTrigger() - " + _trigger.text);

        //Keep track that the player has met this trigger
        MeetCondition(_trigger.text);

        //Keep track that the player has met this specific colour
        if (_trigger.isColourTrigger()) {
            MeetCondition(_trigger.colour);
        }
        
        //Also keep track of the number of good+bad colours they have acquired
        if (_trigger.isGoodQualityTrigger()) {
            goodColoursCount++;
        } else if (_trigger.isBadQualityTrigger()) {
            badColoursCount++;
        }

        if (isSpecialCondition(_trigger.text)) {
            switch(_trigger.text) {
                case "foundKnife":
                    GameObject.Find("Knife").SetActive(false);
                    break;
                case "clericTransformation":
                    GameObject.Find("Saint_Casey").name = "Casey_The_Heretic";
                    break;
                default:
                    Debug.Log("StoryConditionManager::HandleTrigger() unknown special condition (" + _trigger.text + ")");
                    break;
            }
        }
    }

    /*
        Should be at most one special trigger per list
        TODO tests for this
    */
    public Conversation_Trigger getSpecialTrigger(Conversation_Trigger[] _triggers) {
        foreach (Conversation_Trigger trigger in _triggers) {
            if (trigger.isColourTrigger()) {
                return trigger;
            }
        }
        return null;
    }

    public override bool isSpecialCondition(String _condition) {
        return specialConditions.Contains(_condition);
    }
}
