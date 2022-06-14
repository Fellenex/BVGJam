using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
A fake little class to pack a Conversation_Transition object onto a button
*/
public class TransitionHolder : MonoBehaviour {
    public Conversation_Transition transition;

    void Setup(Conversation_Transition _t) {
        transition = _t;
    }
}
