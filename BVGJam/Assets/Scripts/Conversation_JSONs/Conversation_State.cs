[System.Serializable]
public class Conversation_State {

    public string index;
    public Conversation_Statement[] statements;

    public bool hasMoreStatements(int _index) {
        return _index < statements.Length - 1;
    }

    //Gets the first speaker in a conversation.
    //Assumes the first state is listed first.
    public Conversation_Statement getFirstStatement() {
        return statements[0];
    }
}
