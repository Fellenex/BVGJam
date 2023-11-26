using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ConditionManager : MonoBehaviour {

    public List<String> conditionsMet = new List<String>();

    //A li'l wrapper to set a condition to true and instantiate the list entry if it has yet to be set
    public void MeetCondition(String _condition) {
        if (_condition.Length == 0) {
            Debug.LogWarning("ConditionManager::MeetCondition empty condition");
        }
        Debug.Log("Player has just met condition " + _condition);
        conditionsMet.Add(_condition);
    }

    public bool hasMetCondition(String _condition) {
        Debug.Log("Checking if player meets " + _condition);
        if (isNegativeCondition(_condition)) {
            return !conditionsMet.Contains(_condition.Substring(1, _condition.Length - 1));
        } else { 
            return conditionsMet.Contains(_condition);
        }
    }

    //e.g., "!petDead" can be used to make sure the player has not yet met the condition "petDead"
    private bool isNegativeCondition(String _condition) {
        return _condition[0] == '!';
    }

    public void prettyPrintConditions() {
        foreach (String x in conditionsMet) {
            Debug.Log(x + " --> " + x);
        }
    }

    public void HandleTriggers(Conversation_Trigger[] _triggers)
    {
        foreach (Conversation_Trigger trigger in _triggers)
        {
            HandleTrigger(trigger);
        }
    }

    /*
    The following methods should be defined in a child class based on what's needed for the story
    */
    public abstract bool isSpecialCondition(String _condition);
    public abstract void HandleTrigger(Conversation_Trigger trigger);
}
