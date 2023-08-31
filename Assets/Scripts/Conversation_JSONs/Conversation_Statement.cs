using System;
using UnityEngine;

[System.Serializable]
public class Conversation_Statement {

    public string speaker;
    public string text;
    public string mood;

    private static string MOOD_PLEASED = "pleased";
    private static string MOOD_NEUTRAL = "neutral";
    private static string MOOD_UPSET = "upset";

    public void validate() {
        //The statement needs to have a speaker
        //For colour states, we don't always have a speaker
        //Debug.Assert(!String.IsNullOrEmpty(speaker));

        //The statement has to be saying something
        Debug.Assert(!String.IsNullOrEmpty(text));

        //We only have 3 moods for speaker graphics
        Debug.Assert(mood == MOOD_PLEASED || mood == MOOD_NEUTRAL || mood == MOOD_UPSET || mood == "");
    }
}
