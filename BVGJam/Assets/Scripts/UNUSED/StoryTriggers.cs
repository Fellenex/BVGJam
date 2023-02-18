using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoryTriggers {

    public static string triggerPrefix = "get";
    public static string conditionPrefix = "has";

    public static List<string> triggers;
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
    public static string getYourBlood = "getYourBlood";
    public static string getAssassinsBlood = "getAssassinsBlood";
    //
    public static string getSonicSpore = "getSonicSpore";
    public static string getLakewaterSpore = "getLakewaterSpore";
    public static string getCursedSpore = "getCursedSpore";

    //Cleric-relevant triggers
    public static string getClericsBlessing = "getClericsBlessing";
    public static string getClericsMourning = "getClericsMourning";
    public static string getCursedAura = "getCursedAura";
    public static string getEvilAura = "getEvilAura";
    public static string getPaladinRevived = "getPaladinRevived";
    public static string getMetCleric = "getMetCleric";

    //Monk-relevant triggers
    public static string getYellowGood = "getYellowGood";
    public static string getYellowBad = "getYellowBad";

    //Other triggers
    public static string getFoundKnife = "getFoundKnife";



    //Parental trigger collections
    public static List<string> colourTriggers; // = [getBlue, getRed, getYellow, getGreen, getPurple];
    public static List<string> blueGoodTriggers;
    public static List<string> redGoodTriggers;
    public static List<string> yellowGoodTriggers;
    public static List<string> greenGoodTriggers;
    public static List<string> purpleGoodTriggers;
    public static List<List<string>> goodTriggers;

    public static List<string> blueBadTriggers;
    public static List<string> redBadTriggers;
    public static List<string> yellowBadTriggers;
    public static List<string> greenBadTriggers;
    public static List<string> purpleBadTriggers;
    public static List<List<string>> badTriggers;

    /*
    Neutral triggers
        blueHunt
        getPaladinDead
        getPaladinRevived
        getMetCleric
        getFoundKnife
    */


    //Used if we have any triggers that don't follow the get-->have regular rule
    public static Dictionary<string,string> mapTriggerToCondition;

    public static void setupTriggers() {
        triggers = new List<string>();

        //Add the overarching colour triggers
        colourTriggers = new List<string>();
        colourTriggers.AddRange( new string[]{getBlue, getRed, getYellow, getGreen, getPurple} );
        foreach (string trigger in colourTriggers) {
            triggers.Add(trigger);
        }

        //Add the neutral triggers
        triggers.Add(getBlueHunt);
        triggers.Add(getPaladinDead);
        triggers.Add(getPaladinRevived);
        triggers.Add(getMetCleric);
        triggers.Add(getFoundKnife);

        colourTriggers = new List<string>(); 
        blueGoodTriggers = new List<string>();
        redGoodTriggers = new List<string>();
        yellowGoodTriggers = new List<string>();
        greenGoodTriggers = new List<string>();
        purpleGoodTriggers = new List<string>();
        goodTriggers = new List<List<string>>();

        blueBadTriggers = new List<string>();
        redBadTriggers = new List<string>();
        yellowBadTriggers = new List<string>();
        greenBadTriggers = new List<string>();
        purpleBadTriggers = new List<string>();
        badTriggers = new List<List<string>>();


        //Parental trigger collections
        blueGoodTriggers.AddRange(      new string[]{ getJusticeSweat, getBardsRequiem });
        redGoodTriggers.AddRange(       new string[]{ getPaladinBloodGood, getYourBlood, getAssassinsBlood});
        yellowGoodTriggers.AddRange(    new string[]{ getYellowGood });
        greenGoodTriggers.AddRange(     new string[]{ getSonicSpore, getLakewaterSpore });
        purpleGoodTriggers.AddRange(    new string[]{ getClericsBlessing, getClericsMourning });
        //
        goodTriggers = new List<List<string>>();
        goodTriggers.AddRange( new List<string>[]{
            blueGoodTriggers,
            redGoodTriggers,
            yellowGoodTriggers,
            greenGoodTriggers,
            purpleGoodTriggers});
        //
        foreach (List<string> triggerArray in goodTriggers) {
            foreach (string trigger in triggerArray){
                triggers.Add(trigger);
            }
        }

        blueBadTriggers.AddRange(    new string[]{ getExistentialSweat, getToxicSweat });
        redBadTriggers.AddRange(     new string[]{ getPaladinBloodBad });
        yellowBadTriggers.AddRange(  new string[]{ getYellowBad });
        greenBadTriggers.AddRange(   new string[]{ getCursedSpore });
        purpleBadTriggers.AddRange(  new string[]{ getCursedAura, getEvilAura});
        //
        badTriggers = new List<List<string>>();
        badTriggers.AddRange (new List<string>[]{
            blueBadTriggers,
            redBadTriggers,
            yellowBadTriggers,
            greenBadTriggers,
            purpleBadTriggers});
        //
        foreach (List<string> triggerArray in badTriggers) {
            foreach (string trigger in triggerArray){
                triggers.Add(trigger);
            }
        }

        //trigger(getFoundKnife);
        //trigger(getToxicSweat);
    }


    public static void trigger(string _triggerName) {
        Debug.Log("Triggering: " + _triggerName);
        Dictionary<string, string> dData = new Dictionary<string, string>();

        //TODO here is where to put the dramatic dialog initiation

        //Setup the parent colour triggers to fire off as well
        if (blueGoodTriggers.Contains(_triggerName) || blueBadTriggers.Contains(_triggerName)) {
            
            //if this is the player's first time getting blue, do some extra triggers
            if (!StoryConditions.playerMeetsCondition(getBlue)) {
                trigger(getBlue);
            }
            
        }
        if (redGoodTriggers.Contains(_triggerName) || redBadTriggers.Contains(_triggerName)) {
            trigger(getRed);

            //if this is the player's first time getting red, do some extra triggers
            if (!StoryConditions.playerMeetsCondition(getRed)) {
                trigger(getRed);
            }
        }
        if (yellowGoodTriggers.Contains(_triggerName) || yellowBadTriggers.Contains(_triggerName)) {
            trigger(getYellow);

            //if this is the player's first time getting yellow, do some extra triggers
            if (!StoryConditions.playerMeetsCondition(getYellow)) {
                trigger(getYellow);
            }
        }
        if (greenGoodTriggers.Contains(_triggerName) || greenBadTriggers.Contains(_triggerName)) {
            trigger(getGreen);

            //if this is the player's first time getting green, do some extra triggers
            if (!StoryConditions.playerMeetsCondition(getGreen)) {
                trigger(getGreen);
            }
        }
        if (purpleGoodTriggers.Contains(_triggerName) || purpleBadTriggers.Contains(_triggerName)) {
            trigger(getPurple);

            //if this is the player's first time getting purple, do some extra triggers
            if (!StoryConditions.playerMeetsCondition(getPurple)) {
                trigger(getPurple);
            }
        }
        

        if(_triggerName == getBlue){
            //Disable the hunt for blue, if blue is found
            StoryConditions.playerHasUnmetCondition(convertTriggerIntoCondition(getBlueHunt));

            //special case. can't use switch because it thinks it'll change
            //could always make the above variables constant to fix this,
            //  but i don't think we have very many special triggers
            //      (except::: probably want to handle metaconditions here for conversation_id swapping)
        }
        else{
            //Unless there was a special case, we just turn
            //  "get<TRIGGER>" into "have<TRIGGER>"
            StoryConditions.playerHasMetCondition(convertTriggerIntoCondition(_triggerName));
        }
    }

    //A li'l helper function to turn trigger names into condition names.
    //e.g., we turn "getToxicSweat" into "haveToxicSweat";
    private static string convertTriggerIntoCondition(string _triggerName){
        string returnString = conditionPrefix + _triggerName.Substring(triggerPrefix.Length, _triggerName.Length - triggerPrefix.Length);
        Debug.Log(returnString);
        return(returnString);
    }
}
