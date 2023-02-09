using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetNextConversationID {

    public static Dictionary<string, string> currentConversationPerNPC;
    public static Dictionary<string,string[]> dialogMap;

    public static void setupDialogIDMappings(){ 
        currentConversationPerNPC = new Dictionary<string,string>();
        currentConversationPerNPC.Add("Paladin", "Paladin_A");
        currentConversationPerNPC.Add("Cleric", "Cleric_A");
        currentConversationPerNPC.Add("Bard", "Bard_A");
        currentConversationPerNPC.Add("Monk", "Monk_A");
        currentConversationPerNPC.Add("Druid", "Druid_A");


        dialogMap = new Dictionary<string, string[]>();
    
        dialogMap["Paladin_A"] = new string[]{"Paladin_B"};
        //dialogMap["Paladin_A_A_A"] = new string[]{"Paladin_B"};
        //dialogMap["Paladin_A_B_A"] = new string[]{"Paladin_C"};
        dialogMap["Paladin_B"] = new string[]{};
        dialogMap["Paladin_C"] = new string[]{"Paladin_D"};
        dialogMap["Paladin_D"] = new string[]{};
        dialogMap["Paladin_E"] = new string[]{"Paladin_G"};
        dialogMap["Paladin_G"] = new string[]{};

        dialogMap["Cleric_A"] = new string[]{"Cleric_B"};
        //dialogMap["Cleric_A_A"] = new string[]{"Cleric_B"};
        dialogMap["Cleric_B"] = new string[]{"Cleric_C"};
        //dialogMap["Cleric_B_A"] = new string[]{"Cleric_C"};
        dialogMap["Cleric_C"] = new string[]{};

        dialogMap["Bard_A"] = new string[]{};
        dialogMap["Monk_A"] = new string[]{};
        dialogMap["Druid_A"] = new string[]{}; //maybe assassin
    }
}
