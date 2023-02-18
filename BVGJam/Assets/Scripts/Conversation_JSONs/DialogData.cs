using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogData {
    
    public Conversation[] conversations;

    public static DialogData CreateFromJSON(string data) {
        return JsonUtility.FromJson<DialogData>(data);
    }
}
