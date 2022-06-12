using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogDataJSON {
    
    public Conversation[] conversations;

    public static DialogDataJSON CreateFromJSON(string data) {
        return JsonUtility.FromJson<DialogDataJSON>(data);
    }
}
