using System;
using UnityEngine;

[System.Serializable]
public class Conversation_State {

    public string index;
    public Conversation_Statement[] statements;

    public bool hasMoreStatements(int _index) {
        return _index < statements.Length;
    }

    //Gets the first speaker in a conversation.
    //Assumes the first state is listed first.
    public Conversation_Statement getFirstStatement() {
        return statements[0];
    }

    public void validate() {
        //Every state needs to have an index so we can reference it
        Debug.Assert(!String.IsNullOrEmpty(index));

        foreach (Conversation_Statement statement in statements) {
            statement.validate();
        }
    }
}
