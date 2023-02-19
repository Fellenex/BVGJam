using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StoryConditions {

    //Keep track of which NPC conversations have been entered.
    //  Maps an NPC name to a list of conversation IDs
    public static Dictionary<String, List<String>> npcConversations = new Dictionary<String, List<String>>();

    public static List<String> conditions = new List<String>();
    public enum ConversationStatus { Uninitiated, Started, Finished };

    public static Dictionary<ValueTuple<String, String>, ConversationStatus> conversationStatus = new Dictionary<ValueTuple<String, String>, ConversationStatus>();

    //public static Dictionary<string, ConversationStatus> conversationStatus = new Dictionary<string, ConversationStatus>();

    public static int goodColoursCount = 0;
    public static int badColoursCount = 0;


    public static bool playerMeetsCondition(String _condition) {
        Debug.Log("Checking if player meets " + _condition);
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
    private static bool checkCondition(String _condition) {
        //Debug.Log("Checking if player has met condition " + _condition);
        return conditions.Contains(_condition);
    }

    //A li'l wrapper to set a condition to true and instantiate the list entry if it has yet to be set
    public static void playerHasMetCondition(String _condition){
        Debug.Log("Player has just met condition " + _condition);
        conditions.Add(_condition);
    }

    //And a second li'l wrapper to set a condition to true and instantiate the list entry otherwise
    public static void playerHasUnmetCondition(String _condition){
        try { conditions.Remove(_condition); }
        catch (Exception e) { Debug.Log("Couldn't remove condition " + _condition); }
    }

    public static void StartConversation(String _npcName, String _conversationId){
        try {
            npcConversations[_npcName].Add(_conversationId);
        }
        catch (KeyNotFoundException e) {
            npcConversations[_npcName] = new List<string>();
            npcConversations[_npcName].Add(_conversationId);
        }
        conversationStatus[(_npcName, _conversationId)] = ConversationStatus.Started;
    }

    public static void FinishConversation(String _npcName, String _conversationId){
        Debug.Log("Finishing conversation " + _conversationId + " with " + _npcName);
        conversationStatus[(_npcName, _conversationId)] = ConversationStatus.Finished;
    }

    //A li'l wrapper to safely check if a conversation has been completed
    public static bool hasFinishedConversation(String _npcName, String _conversationId) {
        try {
            return conversationStatus[(_npcName, _conversationId)] == ConversationStatus.Finished;
        } catch (KeyNotFoundException) {
            conversationStatus[(_npcName, _conversationId)] = ConversationStatus.Uninitiated;
            return false;
        }
    }
}
