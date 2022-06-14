using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupConditionsAndTriggers : MonoBehaviour {
    //Setup the global configuration for the game
    void Start() {
        StoryConditions.setupConditionMapping();
        StoryTriggers.setupTriggerMapping();
    }
}
