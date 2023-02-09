using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DramaticDialog {

    public static Dictionary<string, Color> mapTriggerToColor;

    //Store arrays of strings instead of a single string so that we can display things on multiple lines
    public static Dictionary<string, string[]> mapTriggerToTexts;

    private static Color redGood;
    private static Color redBad;
    private static Color blueGood;
    private static Color blueBad;
    private static Color yellowGood;
    private static Color yellowBad;
    private static Color purpleGood;
    private static Color purpleBad;
    private static Color greenGood;
    private static Color greenBad;

    public static void setupDramaticDialogs() {
        redGood = new Color(248,25,0,255);
        redBad = new Color(199,120,120,255);
        blueGood = new Color(11,3,252,255);
        blueBad = new Color(148,183,194,255);
        yellowGood = new Color(255,255,51,255);
        yellowBad = new Color(219,224,182,255);
        purpleGood = new Color(177,0,231,255);
        purpleBad = new Color(99,33,133,255);
        greenGood = new Color(0,232,31,255);
        greenBad = new Color(209,237,119,255);

        mapTriggerToColor = new Dictionary<string, Color>();
        mapTriggerToTexts = new Dictionary<string, string[]>();

        setupColorMappings();
        setupTextMappings();

    }

    public static void setupColorMappings() {
        //Blue mappings
        mapTriggerToColor.Add(StoryTriggers.getJusticeSweat, blueGood);
        mapTriggerToColor.Add(StoryTriggers.getBardsRequiem, blueGood);
        //
        mapTriggerToColor.Add(StoryTriggers.getToxicSweat, blueBad);
        mapTriggerToColor.Add(StoryTriggers.getExistentialSweat, blueBad);

        //Purple mappings        
        mapTriggerToColor.Add(StoryTriggers.getClericsBlessing, purpleGood);
        mapTriggerToColor.Add(StoryTriggers.getClericsMourning, purpleGood);
        //
        mapTriggerToColor.Add(StoryTriggers.getEvilAura, purpleBad);
        mapTriggerToColor.Add(StoryTriggers.getCursedAura, purpleBad);

        //Red mappings
        mapTriggerToColor.Add(StoryTriggers.getPaladinBloodGood, redGood);
        mapTriggerToColor.Add(StoryTriggers.getYourBlood, redGood);
        mapTriggerToColor.Add(StoryTriggers.getAssassinsBlood, redGood);
        //
        mapTriggerToColor.Add(StoryTriggers.getPaladinBloodBad, redBad);
        
        //Yellow mappings
        mapTriggerToColor.Add(StoryTriggers.getYellowGood, yellowGood);
        mapTriggerToColor.Add(StoryTriggers.getYellowBad, yellowBad);

        //Green mappings
        mapTriggerToColor.Add(StoryTriggers.getSonicSpore, greenGood);
        mapTriggerToColor.Add(StoryTriggers.getLakewaterSpore, greenGood);
        //
        mapTriggerToColor.Add(StoryTriggers.getCursedSpore, greenBad);
    }

    public static void setupTextMappings() {
        //Blue mappings
        mapTriggerToTexts.Add(StoryTriggers.getJusticeSweat,
            new string[]{"You’ve obtained The Paladin’s Toxic Masculinity Sweat!"
            });
        mapTriggerToTexts.Add(StoryTriggers.getBardsRequiem,
            new string[]{"As the sun goes down, on this Blue night.",
                "A friend cries eternal, tears for the knight.",
                "A solemn hymn, for a solid him.",
                "A moment grim, from the Reaper’s whim.",
                "",
                "You stomached The Bard’s Requiem!"
            });
        //
        mapTriggerToTexts.Add(StoryTriggers.getToxicSweat, 
            new string[]{"...I don’t know if I am capable of feeling happiness.",
                "You received The Paladin’s Existential Sweat!"
            });
        mapTriggerToTexts.Add(StoryTriggers.getExistentialSweat,
            new string[]{"You obtain a few droplets of Jonathan’s profuse J U S T I C E  S W E A T."});


        //Purple mappings        
        mapTriggerToTexts.Add(StoryTriggers.getClericsBlessing,
            new string[]{"Saint Casey heals you with her Cleric magic.",
                "You receive "+'"'+"Cleric's Blessing"+'"'+"!",
                "And also you stop bleeding!"});


        mapTriggerToTexts.Add(StoryTriggers.getClericsMourning,
            new string[]{"Sir Jonathan’s soul rests and finds eternal peace as a Purple tear slides down Saint Casey’s face.",
                '"'+"Rest easy...friend."+'"',
                "You obtain Cleric's Mourning"});
        //
        mapTriggerToTexts.Add(StoryTriggers.getEvilAura, new string[]{
            "Saint Casey begins to speak in tongues as darkness suffocates the sky. The clouds are split asunder, as a primordial moan echoes across space and time.",
            "Saint Casey has broken her holy vow.",
            "You have obtained an Evil Cleric's Aura!"
        });
        mapTriggerToTexts.Add(StoryTriggers.getCursedAura, new string[]{
            "Saint Casey begins to speak in tongues as darkness suffocates the sky. For a brief moment, Sir Jonathan can be heard screaming "+'"'+"Put me back!"+'"',
            "Saint Casey has broken her holy vow.",
            "You have obtained a Cursed Cleric's Aura"
        });

        /*

        //Red mappings
        mapTriggerToTexts.Add(StoryTriggers.getPaladinBloodGood, redGood);
        mapTriggerToTexts.Add(StoryTriggers.getYourBlood, redGood);
        mapTriggerToTexts.Add(StoryTriggers.getAssassinsBlood, redGood);
        //
        mapTriggerToTexts.Add(StoryTriggers.getPaladinBloodBad, redBad);
        
        //Yellow mappings
        mapTriggerToTexts.Add(StoryTriggers.getYellowGood, yellowGood);
        mapTriggerToTexts.Add(StoryTriggers.getYellowBad, yellowBad);

        //Green mappings
        mapTriggerToTexts.Add(StoryTriggers.getSonicSpore, greenGood);
        mapTriggerToTexts.Add(StoryTriggers.getLakewaterSpore, greenGood);
        //
        mapTriggerToTexts.Add(StoryTriggers.getCursedSpore, greenBad);


        */
    }
}