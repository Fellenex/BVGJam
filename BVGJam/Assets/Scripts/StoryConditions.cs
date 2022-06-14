using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//TODO gotta have some other script that checks the secondary conditions
//  and assigns their truth value based on what is happening in-game

public static class StoryConditions {

    //Keep track of which NPC conversations have been entered.
    //  Maps an NPC name to a list of conversation IDs
    public static Dictionary<string, List<string>> npcConversations;
    
    //Keep track of which conversations have been completed
    //  Maps a conversation ID to a truth value
    public static Dictionary<string, bool> conversationCompleted;

    //A holder for the remaining conditions not otherwise covered by conversation status
    public static Dictionary<string, bool> secondaryConditions;


    /*
    We don't need to populate any of the default conditions, since
        we automatically set them as false if we are checking the condition
        and it has yet to be added to secondaryConditions.
    */


    public static void setupConditionMapping(){
        //TODO set up any conditions we want to do additional things with here
        npcConversations = new Dictionary<string, List<string>>();
        conversationCompleted = new Dictionary<string, bool>();
        //threadStatus = new Dictionary<string, bool>();
        secondaryConditions = new Dictionary<string, bool>();
    }


    public static bool doesPlayerMeetCondition(string _condition) {
        if (_condition[0] == '!'){ 
            //We want to check that the player has /not/ met the condition
            return(!checkCondition(_condition.Substring(1, _condition.Length-1)));
        }
        else{ 
            //We want to check that the player /has/ met the condition
            return(checkCondition(_condition));
        }
    }

    //Returns bool indicating whether or not _condition is set to true in secondaryConditions
    //A li'l wrapper to check a condition and instantiate the list entry if it has yet to be set
    private static bool checkCondition(string _condition) {
        bool truthValue;
        try {
            truthValue = secondaryConditions[_condition];
        }
        catch (Exception e){
            truthValue = false;
            secondaryConditions.Add(_condition, truthValue);
        }
        return(truthValue);
    }

    //A li'l wrapper to set a condition to true and instantiate the list entry if it has yet to be set
    public static void playerHasMetCondition(string _condition){
        try {
            secondaryConditions[_condition] = true;
        }
        catch (Exception e) {
            secondaryConditions.Add(_condition, true);
        }
    }

    public static void startConversation(string _npcName, string _conversationId){
        try {
            npcConversations[_npcName].Add(_conversationId);
        }
        catch (KeyNotFoundException e) {
            npcConversations[_npcName] = new List<string>();
            npcConversations[_npcName].Add(_conversationId);
        }
        conversationCompleted[_conversationId] = false;
    }
    public static void finishConversation(string _conversationId){
        conversationCompleted[_conversationId] = true;
    }

    //A li'l wrapper to safely check if a conversation has been completed
    public static bool hasFinishedConversation(string _conversationId) {
        try {
            return conversationCompleted[_conversationId];
        }
        catch (KeyNotFoundException) {
            return false;
        }
    }
}
