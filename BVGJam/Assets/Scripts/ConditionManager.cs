﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ConditionManager {

    public static List<String> conditions = new List<String>();


    //A li'l wrapper to set a condition to true and instantiate the list entry if it has yet to be set
    public static void MeetCondition(String _condition) {
        if (_condition.Length == 0) {
            Debug.LogWarning("ConditionManager::MeetCondition empty condition");
        }
        Debug.Log("Player has just met condition " + _condition);
        conditions.Add(_condition);
    }

    public static bool hasMetCondition(String _condition) {
        Debug.Log("Checking if player meets " + _condition);
        if (isNegativeCondition(_condition)) {
            //We want to check that the player has /not/ met the condition
            return !conditions.Contains(_condition.Substring(1, _condition.Length - 1));
        } else { 
            //We want to check that the player /has/ met the condition
            return conditions.Contains(_condition);
        }
    }

    //Currently in the json we use (e.g.,) !paladinDead to mean "not paladinDead"
    private static bool isNegativeCondition(String _condition) {
        return _condition[0] == '!';
    }

    public static void prettyPrintConditions() {
        foreach (String x in conditions) {
            Debug.Log(x + " --> " + x);
        }
    }
}
