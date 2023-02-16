﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StoryConditions {

    //Keep track of which NPC conversations have been entered.
    //  Maps an NPC name to a list of conversation IDs
    public static Dictionary<string, List<string>> npcConversations = new Dictionary<string, List<string>>();

    //Keep track of which NPC conversation should be given next
    public static Dictionary<string, string> nextConversationIdByActor;
    public static Dictionary<string, string> currentConversationPerNPC;
    public static Dictionary<string,string[]> dialogMap;


    public static List<string> conditions = new List<string>();
    public enum ConversationStatus { Uninitiated, Started, Finished };
    public static Dictionary<string, ConversationStatus> conversationStatus = new Dictionary<string, ConversationStatus>();

    

    public static bool playerMeetsCondition(string _condition) {
        if (_condition[0] == '!'){ 
            //We want to check that the player has /not/ met the condition
            return !checkCondition(_condition.Substring(1, _condition.Length-2));
        }
        else{ 
            //We want to check that the player /has/ met the condition
            return checkCondition(_condition);
        }
    }

    //Returns bool indicating whether or not _condition is set to true in secondaryConditions
    //A li'l wrapper to check a condition and instantiate the list entry if it has yet to be set
    private static bool checkCondition(string _condition) {
        Debug.Log("Checking if player has met condition " + _condition);
        return conditions.Contains(_condition);
    }

    //A li'l wrapper to set a condition to true and instantiate the list entry if it has yet to be set
    public static void playerHasMetCondition(string _condition){
        //TODO should handle all the colour stuff here. Take a Conversation_Trigger instead a string here
        Debug.Log("Player has just met condition " + _condition);
        conditions.Add(_condition);
    }

    //And a second li'l wrapper to set a condition to true and instantiate the list entry otherwise
    public static void playerHasUnmetCondition(string _condition){
        try { conditions.Remove(_condition); }
        catch (Exception e) { Debug.Log("Couldn't remove condition " + _condition); }
    }

    public static void StartConversation(string _npcName, string _conversationId){
        try {
            npcConversations[_npcName].Add(_conversationId);
        }
        catch (KeyNotFoundException e) {
            npcConversations[_npcName] = new List<string>();
            npcConversations[_npcName].Add(_conversationId);
        }
        conversationStatus[_conversationId] = ConversationStatus.Started;
    }

    public static void finishConversation(string _conversationId, string _npcClass){
        conversationStatus[_conversationId] = ConversationStatus.Finished;

        try{
            //TODO assumes there's only ever one follow-up conversation
            //nextConversationIdByActor[_npcClass] = dialogMap[_conversationId][0];
        }
        catch (Exception e) {
            //no more available conversations, just move along
        }
    }

    //A li'l wrapper to safely check if a conversation has been completed
    
    //TODO take an npc name and a conversation ID so that they dont' have to be unique between NPCs
    public static bool hasFinishedConversation(string _conversationId) {
        try {
            return conversationStatus[_conversationId] == ConversationStatus.Finished;
            }
        catch (KeyNotFoundException) {
            conversationStatus[_conversationId] = ConversationStatus.Uninitiated;
            return false;
        }
    }
}
