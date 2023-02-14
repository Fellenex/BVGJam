using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupConditionsAndTriggers : MonoBehaviour {
    //Setup the global configuration for the game
    void Awake() {
        GetNextConversationID.setupDialogIDMappings();
        StoryTriggers.setupTriggers();
        DramaticDialog.setupDramaticDialogs();
    }
}
