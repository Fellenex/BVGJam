using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryConditionManager : ConditionManager {

    private readonly String GOOD_QUALITY = "good";
    private readonly String BAD_QUALITY = "bad";
    private readonly List<String> COLOURS = new List<String> { "blue", "red", "green", "purple", "yellow" };

    private int goodColoursCount = 0;
    private int badColoursCount = 0;


    //A list of conditions that cause us to do something in code
    private List<String> specialConditions = new List<String>{
        "foundKnife",               //Make the knife disappear when we "pick it up"
        "clericTransformation"      //Change the Cleric's name from "Casey_The_Cleric" to "Casey_The_Heretic"
    };


    /*
    Returns true if it was a special trigger
    */
    public override void HandleTrigger(Conversation_Trigger _trigger) {
        Debug.Log("StoryConditionManager::HandleTrigger() - " + _trigger.text);

        //Keep track that the player has met this trigger
        MeetCondition(_trigger.text);

        //Keep track that the player has met this specific colour
        if (isColourTrigger(_trigger)) {
            MeetCondition(_trigger.colour);
        }
        
        //Also keep track of the number of good+bad colours they have acquired
        if (isGoodQualityTrigger(_trigger)) {
            goodColoursCount++;
        } else if (isBadQualityTrigger(_trigger)) {
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

    public override bool isSpecialCondition(String _condition) {
        return specialConditions.Contains(_condition);
    }
    public bool isColourTrigger(Conversation_Trigger _trigger) {
        return (!String.IsNullOrEmpty(_trigger.quality) && !String.IsNullOrEmpty(_trigger.colour));
    }

    
    public bool isGoodQualityTrigger(Conversation_Trigger _trigger) { return _trigger.text == GOOD_QUALITY; }
    public bool isBadQualityTrigger(Conversation_Trigger _trigger) { return _trigger.text == BAD_QUALITY; }

    /*
    Should be at most one colour trigger per list
    TODO test for this
    */
    public Conversation_Trigger getSpecialTrigger(Conversation_Trigger[] _triggers) {
        foreach (Conversation_Trigger trigger in _triggers) {
            if (isColourTrigger(trigger)) {
                return trigger;
            }
        }
        return null;
    }

    

    /*
        //If we have a quality attribute, then we need to have a colour attribute
        //(Not necessarily the other way around)
        if (!string.IsNullOrEmpty(quality)) {
            Debug.Assert(!String.IsNullOrEmpty(colour));

            Debug.Assert(isGoodQualityTrigger() || isBadQualityTrigger() || isNoneQualityTrigger());
        }
    */

    
}
