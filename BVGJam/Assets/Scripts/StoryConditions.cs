using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StoryConditions {

    public static int goodColoursCount = 0;
    public static int badColoursCount = 0;

    public static List<String> specialConditions = new List<String>{
        "foundKnife",               //Make the knife disappear when we "pick it up"
        "clericTransformation"      //Change the Cleric's name from "Casey_The_Cleric" to "Casey_The_Heretic"
        };


    

    public static bool isSpecialCondition(String _condition) {
        return specialConditions.Contains(_condition);
    }
    
}
