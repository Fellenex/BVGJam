using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHolder : MonoBehaviour
{
    public Conversation_Transition transition;

    void Setup(Conversation_Transition _t) {
        transition = _t;
    }
}
