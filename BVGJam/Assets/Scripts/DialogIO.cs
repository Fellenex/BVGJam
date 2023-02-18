using System;
using System.Collections.Generic;
using UnityEngine;

public static class DialogIO {

    //validateDialogFile(readWholeConversationFile(jsonToTest));

    public static DialogData CreateFromJSON(string _data) {
        return JsonUtility.FromJson<DialogData>(_data);
    }

    //Short and sweet. Get the whole JSON
    public static DialogData ReadDialogFile(TextAsset _conversationsFile) {
        String fileContents = _conversationsFile.ToString();
        DialogData convos = CreateFromJSON(fileContents);
        return convos;
    }
}
