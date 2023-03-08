using System;
using System.Collections.Generic;
using UnityEngine;

public static class DialogIO {

    //validateDialogFile(readWholeConversationFile(jsonToTest));

    public static List<Conversation>CreateFromJSON(string _data) {
        return JsonUtility.FromJson<List<Conversation>>(_data);
    }

    //Short and sweet. Get the whole JSON
    public static List<Conversation> ReadDialogFile(TextAsset _conversationsFile) {
        String fileContents = _conversationsFile.ToString();
        List<Conversation> convos = CreateFromJSON(fileContents);
        return convos;
    }
}
