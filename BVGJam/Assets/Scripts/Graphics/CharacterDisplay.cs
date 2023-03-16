using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CharacterDisplay {

    public string codeName { get; private set; }
    public string displayName { get; private set; }
    public Dictionary<string, Sprite> images { get; private set; }
    public Vector2 dimensions { get; private set; }



    private static List<string> moods = new List<string> { "neutral", "pleased", "upset" };
    private static int NUM_MOODS = moods.Count;

    private static string PAL_NAME = "Pal";
    private static string CASEY_NAME = "Saint_Casey";
    private static string CASEY_NAME_TWO = "Casey_The_Heretic";
    private static string JOE_NAME = "Sir_Jonathan";
    private static string MARK_NAME = "Mark_The_Bard";
    private static string MAVERICK_NAME = "Maverick_The_Monk";
    private static string NICKI_NAME = "Nyx";
    private static string GRANDMA_NAME = "Grandma_Yaga";
    private static string KNIFE_NAME = "Knife";

    public static List<string> DISPLAY_NAMES = new List<string> {
        PAL_NAME, CASEY_NAME, CASEY_NAME_TWO, JOE_NAME, MARK_NAME, MAVERICK_NAME, NICKI_NAME, GRANDMA_NAME, KNIFE_NAME
    };

    private static Dictionary<string, string> rootFilepath = new Dictionary<string, string> {
        {PAL_NAME, "Sprites/Player/dialogIcon_"},
        {CASEY_NAME, "Sprites/Casey/dialogIcon_"},
        {CASEY_NAME_TWO, "Sprites/Casey/dialogIcon_"},
        {JOE_NAME, "Sprites/Joe/dialogIcon_"},
        {MARK_NAME, "Sprites/Mark/dialogIcon_"},
        {MAVERICK_NAME, "Sprites/Maverick/dialogIcon_"},
        {NICKI_NAME, "Sprites/Nicki/dialogIcon_"},
        {GRANDMA_NAME, "Sprites/Grandma_Yaga/dialogIcon_"},
        {KNIFE_NAME, "Sprites/Knife/dialogIcon_"}
    };

    private static Dictionary<string, Vector2> dimensionsByCharacter = new Dictionary<string, Vector2> {
        {PAL_NAME, new Vector2(400, 700)},
        {CASEY_NAME, new Vector2(400,750)},
        {CASEY_NAME_TWO, new Vector2(400,750)},
        {JOE_NAME, new Vector2(425, 750)},
        {MARK_NAME, new Vector2(425, 750)},
        {MAVERICK_NAME, new Vector2(400, 700)},
        {NICKI_NAME, new Vector2(500,500)},
        {GRANDMA_NAME, new Vector2(600,750)},
        {KNIFE_NAME, new Vector2(250,250)}
    };

    public CharacterDisplay(string _codeName, string _displayName, Dictionary<string, Sprite> _images, Vector2 _dimensions) {
        codeName = _codeName;
        displayName = _displayName;
        images = _images;
        dimensions = _dimensions;
    }

    public static CharacterDisplay ConstructByName(string _name) {
        if (!DISPLAY_NAMES.Contains(_name)) {
            Debug.LogError("CharacterDisplay::ConstructByName() unknown name (" + _name + ")");
        }

        //Collect the relevant images
        Dictionary<string, Sprite> currImages = new Dictionary<string, Sprite>();
        foreach (string mood in CharacterDisplay.moods){
            //Debug.Log("about to load '"+CharacterDisplay.rootFilepath[_name] + mood);
            currImages[mood] = Resources.Load<Sprite>(CharacterDisplay.rootFilepath[_name] + mood);
        }

        //Construct the object
        return new CharacterDisplay(
            _name,
            _name.Replace("_", " "),
            currImages,
            CharacterDisplay.dimensionsByCharacter[_name]
        );
    }
}