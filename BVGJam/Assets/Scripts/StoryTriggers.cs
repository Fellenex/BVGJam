using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoryTriggers {
    public static string[] triggers;
    
    //Overarching colour triggers
    public static string getBlue = "getBlue";
    public static string getRed = "getRed";
    public static string getYellow = "getYellow";
    public static string getGreen = "getGreen";
    public static string getPurple = "getPurple";

    //Paladin-relevant triggers
    public static string getJusticeSweat = "getJusticeSweat";
    public static string getToxicSweat = "getToxicSweat";
    public static string getExistentialSweat = "getExistentialSweat";
    //
    public static string getBardsRequiem = "getBardsRequiem";
    public static string getBlueHunt = "getBlueHunt";

    //Assassin-relevant triggers
    public static string getPaladinDead = "getPaladinDead";
    //
    public static string getPaladinBloodGood = "getPaladinBloodGood";
    public static string getPaladinBloodBad = "getPaladinBloodBad";
    public static string getYourBloood = "getYourBlood";
    public static string getAssassinsBlood = "getAssassinsBlood";
    //
    public static string getSonicSpore = "getSonicSpore";
    public static string getLakewaterSpore = "getLakewaterSpore";
    public static string getCursedSpore = "getCursedSpore";

    //Cleric-relevant triggers
    public static string getClericsBlessing = "getClericsBlessing";
    public static string getClericsMourning = "getClericsMourning";
    public static string getCursedAura = "getCursedAura";
    public static string getPaladinRevived = "getPaladinRevived";
    public static string getMetCleric = "getMetCleric";

    //Monk-relevant triggers
    public static string getYellowGood = "getYellowGood";
    public static string getYellowBad = "getYellowBad";

    //Other triggers
    public static string getFoundKnife = "getFoundKnife";
    public static string getYourBlood = "getYourBlood";



    //Parental trigger collections
    public static string[] colourTriggers; // = [getBlue, getRed, getYellow, getGreen, getPurple];
    public static string[] blueGoodTriggers;
    public static string[] redGoodTriggers;
    public static string[] yellowGoodTriggers;
    public static string[] greenGoodTriggers;
    public static string[] purpleGoodTriggers;
    public static string[][] goodTriggers;

    public static string[] blueBadTriggers;
    public static string[] redBadTriggers;
    public static string[] yellowBadTriggers;
    public static string[] greenBadTriggers;
    public static string[] purpleBadTriggers;
    public static string[][] badTriggers;

    /*Neutral triggers
        blueHunt
        getPaladinDead
        getPaladinRevived
        getMetCleric  
    */

    public static Dictionary<string,string> mapTriggerToCondition;

    public static void setupTriggers() {
        mapTriggerToCondition = new Dictionary<string, string>();
        colourTriggers = new string[]{getBlue, getRed, getYellow, getGreen, getPurple};

        //Parental trigger collections
        blueGoodTriggers =   new string[]{ getJusticeSweat, getBardsRequiem };
        redGoodTriggers =    new string[]{ getPaladinBloodGood, getYourBlood, getAssassinsBlood};
        yellowGoodTriggers = new string[]{ getYellowGood };
        greenGoodTriggers =  new string[]{ getSonicSpore, getLakewaterSpore, getGreen };
        purpleGoodTriggers = new string[]{ getClericsBlessing, getClericsMourning };

        goodTriggers = new string[][]{
            blueGoodTriggers,
            redGoodTriggers,
            yellowGoodTriggers,
            greenGoodTriggers,
            purpleGoodTriggers
        };

        blueBadTriggers =    new string[]{ getExistentialSweat, getToxicSweat };
        redBadTriggers =     new string[]{ getPaladinBloodBad };
        yellowBadTriggers =  new string[]{ getYellowBad };
        greenBadTriggers =   new string[]{ getCursedSpore };
        purpleBadTriggers =  new string[]{ getCursedAura };

        badTriggers = new string[][]{
            blueBadTriggers,
            redBadTriggers,
            yellowBadTriggers,
            greenBadTriggers,
            purpleBadTriggers
        };
    }


    public static void trigger(string _triggerName) {
        Debug.Log("Triggering: " + _triggerName);
        
        if(_triggerName == "oops"){
            //special case. can't use switch because it thinks it'll change
            //could always make the above variables constant to fix this,
            //  but i don't think we have very many special triggers
            //      (except::: probably want to handle metaconditions here for conversation_id swapping)
        }
        else{
            //Unless there was a special case, we just turn
            //  "get<TRIGGER>" into "have<TRIGGER>"

            //TODO more granular, in-case one of the triggers has "get" as a non-prefix substring
            _triggerName.Replace("get", "have");
            StoryConditions.playerHasMetCondition(_triggerName);
        }
    }
}
