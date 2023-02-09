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
    
    //Keep track of which NPC conversation should be given next
    public static Dictionary<string, string> nextConversationIdByActor;
    public static Dictionary<string, string> currentConversationPerNPC;
    public static Dictionary<string,string[]> dialogMap;

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
        nextConversationIdByActor = new Dictionary<string, string>();
        conversationCompleted = new Dictionary<string, bool>();
        secondaryConditions = new Dictionary<string, bool>();

        currentConversationPerNPC = new Dictionary<string,string>();
        currentConversationPerNPC.Add("Paladin", "Paladin_A");
        currentConversationPerNPC.Add("Cleric", "Cleric_A");
        currentConversationPerNPC.Add("Bard", "Bard_A");
        currentConversationPerNPC.Add("Monk", "Monk_A");
        currentConversationPerNPC.Add("Druid", "Druid_A");

        dialogMap = new Dictionary<string, string[]>();
    
        dialogMap["Paladin_A"] = new string[]{"Paladin_B"};
        //dialogMap["Paladin_A_A_A"] = new string[]{"Paladin_B"};
        //dialogMap["Paladin_A_B_A"] = new string[]{"Paladin_C"};
        dialogMap["Paladin_B"] = new string[]{};
        dialogMap["Paladin_C"] = new string[]{"Paladin_D"};
        dialogMap["Paladin_D"] = new string[]{};
        dialogMap["Paladin_E"] = new string[]{"Paladin_G"};
        dialogMap["Paladin_G"] = new string[]{};

        dialogMap["Cleric_A"] = new string[]{"Cleric_B"};
        //dialogMap["Cleric_A_A"] = new string[]{"Cleric_B"};
        dialogMap["Cleric_B"] = new string[]{"Cleric_C"};
        //dialogMap["Cleric_B_A"] = new string[]{"Cleric_C"};
        dialogMap["Cleric_C"] = new string[]{};

        dialogMap["Bard_A"] = new string[]{};
        dialogMap["Monk_A"] = new string[]{};
        dialogMap["Druid_A"] = new string[]{}; //maybe assassin
        
        /*
        string[] classes = new string[]{"Cleric", "Monk", "Bard", "Druid", "Paladin"};
        foreach (string cls in classes) {
            //TODO assumes that there's only one conversation to proceed towards every time
            //Debug.Log(GetNextConversationID.currentConversationPerNPC[cls]);
            Debug.Log(cls);
            //If we get an IndexOutOfRangeException then there are no more conversation IDs for this class
            try{

                Debug.Log("oops: "+cls + ":: "+currentConversationPerNPC[cls]);
                nextConversationIdByActor[cls] = dialogMap[currentConversationPerNPC[cls]][0];
            }
            catch (IndexOutOfRangeException e) {
                nextConversationIdByActor[cls] = "";
            }
        }
        */
        
        nextConversationIdByActor["Cleric"] = currentConversationPerNPC["Cleric"];
        nextConversationIdByActor["Monk"] = currentConversationPerNPC["Monk"];
        nextConversationIdByActor["Bard"] = currentConversationPerNPC["Bard"];
        nextConversationIdByActor["Druid"] = currentConversationPerNPC["Druid"];
        nextConversationIdByActor["Paladin"] = currentConversationPerNPC["Paladin"];
    
    }


    public static bool playerMeetsCondition(string _condition) {
        if (_condition[0] == '!'){ 
            //We want to check that the player has /not/ met the condition
            return(!checkCondition(_condition.Substring(1, _condition.Length-2)));
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
        } catch (Exception e){
            truthValue = false;
            secondaryConditions.Add(_condition, truthValue);
        }
        return(truthValue);
    }

    //A li'l wrapper to set a condition to true and instantiate the list entry if it has yet to be set
    public static void playerHasMetCondition(string _condition){
        try {
            secondaryConditions[_condition] = true;
        } catch (Exception e) {
            secondaryConditions.Add(_condition, true);
        }
    }
    //And a second li'l wrapper to set a condition to true and instantiate the list entry otherwise
    public static void playerHasUnmetCondition(string _condition){
        try {
            secondaryConditions[_condition] = false;
        } catch (Exception e) {
            secondaryConditions.Add(_condition, false);
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
    public static void finishConversation(string _conversationId, string _npcClass){
        conversationCompleted[_conversationId] = true;

        try{
            //TODO assumes there's only ever one follow-up conversation
            nextConversationIdByActor[_npcClass] = dialogMap[_conversationId][0];
        }
        catch (Exception e) {
            //no more available conversations, just move along
        }
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
