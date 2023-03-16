using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ConditionManager {

    //Keep track of which NPC conversations have been entered.
    //  Maps an NPC name to a list of conversation IDs
    public static Dictionary<String, List<String>> npcConversations = new Dictionary<String, List<String>>();

    public static List<String> conditions = new List<String>();
    public enum ConversationStatus { Uninitiated, Started, Finished };

    public static Dictionary<ValueTuple<String, String>, ConversationStatus> conversationStatus = new Dictionary<ValueTuple<String, String>, ConversationStatus>();

    //Keep track of this conversation having started
    public static void StartConversation(String _npcName, String _conversationId){
        try {
            npcConversations[_npcName].Add(_conversationId);
        }
        catch (KeyNotFoundException e) {
            npcConversations[_npcName] = new List<String>();
            npcConversations[_npcName].Add(_conversationId);
        }
        conversationStatus[(_npcName, _conversationId)] = ConversationStatus.Started;
    }

    //Keep track of this conversation having finished so that we won't start it again
    public static void FinishConversation(String _npcName, String _conversationId){
        Debug.Log("Finishing conversation '" + _conversationId + "' with '" + _npcName + "'");
        conversationStatus[(_npcName, _conversationId)] = ConversationStatus.Finished;
    }

    //A li'l wrapper to set a condition to true and instantiate the list entry if it has yet to be set
    public static void MeetCondition(String _condition) {
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
