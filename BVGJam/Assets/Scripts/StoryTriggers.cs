using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoryTriggers {
    public static Dictionary<string,Trigger> triggerMap;

    public enum Trigger {RUDE, KNIFE_PICKUP, TEMPLATE};

    public static void setupTriggerMapping() {
        triggerMap = new Dictionary<string, Trigger>();
        triggerMap["wasRude"] = Trigger.RUDE;
        triggerMap["knifePickup"] = Trigger.KNIFE_PICKUP;
        triggerMap["template"] = Trigger.TEMPLATE;
    }

    public static void trigger(string _triggerName) {
        Debug.Log("Triggering: " + _triggerName);
        switch(triggerMap[_triggerName]) {
            case Trigger.RUDE:
                Debug.Log("Rude");
                break;

            case Trigger.KNIFE_PICKUP:
                //TODO might want more general "add item" trigger? Depends
                StoryConditions.addInventoryItem("Assassin's Knife");
                Debug.Log("Knife");
                break;

            case Trigger.TEMPLATE:
                break;


            //TODO add more triggers in here.
            //Something that can be done by story probably.
            //At least to add in what the triggers will be.
        }
    }
}
