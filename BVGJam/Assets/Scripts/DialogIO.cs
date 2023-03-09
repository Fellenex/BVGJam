using System;
using System.Collections.Generic;
using UnityEngine;

public static class DialogIO {

    
    public static List<Conversation> ReadDialogFile(TextAsset _conversationsFile) {
        String fileContents = _conversationsFile.ToString();
        List<Conversation> convos = new List<Conversation>();

        //JSON needs to be deserialized into a Conversations object.
        //But we don't care about the Conversations object - we want a list of Conversation objects.
        Conversations jsonConversationList = CreateFromJSON(fileContents);
        return jsonConversationList.conversations;
    }

    //First we read in the list of conversation objects, packed into a "conversations" attribute
    private static Conversations CreateFromJSON(string _data) {
        return JsonUtility.FromJson<Conversations>(_data);
    }
}
