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

    public struct Mood {
        public const string NEUTRAL = "neutral";
        public const string PLEASED = "pleased";
        public const string UPSET = "upset";
    }
    public readonly List<string> validMoods = new List<string> { Mood.NEUTRAL, Mood.PLEASED, Mood.UPSET };

    //This data should be placed into scriptable objects
    private const string PAL_NAME = "Pal";
    private const string CASEY_NAME = "Saint_Casey";
    private const string CASEY_NAME_TWO = "Casey_The_Heretic";
    private const string JOE_NAME = "Sir_Jonathan";
    private const string MARK_NAME = "Mark_The_Bard";
    private const string MAVERICK_NAME = "Maverick_The_Monk";
    private const string NICKI_NAME = "Nyx";
    private const string GRANDMA_NAME = "Grandma_Yaga";
    private const string KNIFE_NAME = "Knife";

    public List<string> DISPLAY_NAMES = new List<string> {
        PAL_NAME, CASEY_NAME, CASEY_NAME_TWO, JOE_NAME, MARK_NAME, MAVERICK_NAME, NICKI_NAME, GRANDMA_NAME, KNIFE_NAME
    };

    private Dictionary<string, string> rootFilepath = new Dictionary<string, string> {
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

    private Dictionary<string, Vector2> dimensionsByCharacter = new Dictionary<string, Vector2> {
        {PAL_NAME, new Vector2(400, 700)},
        {CASEY_NAME, new Vector2(400,750)},
        {CASEY_NAME_TWO, new Vector2(400,750)},
        {JOE_NAME, new Vector2(425, 750)},
        {MARK_NAME, new Vector2(425, 750)},
        {MAVERICK_NAME, new Vector2(400, 700)},
        {NICKI_NAME, new Vector2(650,650)},
        {GRANDMA_NAME, new Vector2(600,750)},
        {KNIFE_NAME, new Vector2(250,250)}
    };

    public CharacterDisplay(string _codeName, string _displayName, Dictionary<string, Sprite> _images, Vector2 _dimensions) {
        codeName = _codeName;
        displayName = _displayName;
        images = _images;
        dimensions = _dimensions;
    }

    public CharacterDisplay (string _name) {
        validateName(_name);
        codeName = _name;
        displayName = _name.Replace("_", " ");
        Dictionary<string, Sprite> currImages = new Dictionary<string, Sprite>();
        foreach (string mood in validMoods) {
            //TODO Consider loading all these in at the start instead of loading them every time
            Debug.Log("loading up sprite at " + rootFilepath[_name] + mood);
            currImages[mood] = Resources.Load<Sprite>(rootFilepath[_name] + mood);
        }
        dimensions = dimensionsByCharacter[_name];
        images = currImages;
    }

    private bool validateName(string _name) {
        bool isValid = DISPLAY_NAMES.Contains(_name);
        if (!isValid) {
            Debug.LogError("CharacterDisplay::validateName() unknown name (" + _name + ")");
        }
        return isValid;
    }
}