using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
A fake little class to pack a Conversation_Option object onto a button
Useful in particular to act as a value for the OnClick function to rely on
*/
public class OptionHolder : MonoBehaviour {
    public Conversation_Option option;

    void Setup(Conversation_Option _t) {
        option = _t;
    }
}
