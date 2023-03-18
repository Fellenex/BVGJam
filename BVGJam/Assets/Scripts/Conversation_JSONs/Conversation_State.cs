using System;
using UnityEngine;

[System.Serializable]
public class Conversation_State {

    public string index;
    public Conversation_Statement[] statements;

    /*
    Returns bool corresponding to whether or not
        there are more statements in the current state
    */
    public bool hasMoreStatements(int _statementIndex) {
        return _statementIndex < statements.Length;
    }

    //Gets the first speaker in a conversation.
    //Assumes the first state is listed first.
    public Conversation_Statement getFirstStatement() {
        try {
            return statements[0];
        } catch (IndexOutOfRangeException e) {
            Debug.LogError("State " + index + " has no statements (" + e.Message + ")");
            return null;
        }
    }

    public void validate() {
        //Every state needs to have an index so we can reference it
        Debug.Assert(!String.IsNullOrEmpty(index));

        foreach (Conversation_Statement statement in statements) {
            statement.validate();
        }
    }
}
