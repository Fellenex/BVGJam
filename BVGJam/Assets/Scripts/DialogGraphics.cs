using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogGraphics : MonoBehaviour {

    private string playerName = "Pal";

    private string DEFAULT_MOOD = "neutral";

    public static DialogGraphics instance;

    public Text playerNameplate;
    public Text npcNameplate;
    public Image activePlayerImage;
    public Image activeNPCImage;

    public Button[] playerTextButtonBoxes;

    public Text speechText;

    public Sprite[] playerDialogIcons;
    public Sprite[] npcDialogIcons_casey;
    public Sprite[] npcDialogIcons_joe;
    public Sprite[] npcDialogIcons_mark;
    public Sprite[] npcDialogIcons_maverick;
    public Sprite[] npcDialogIcons_nicki;
    public Sprite[] npcDialogIcons_grandma;

    public float[] dimensions_casey = new float[]{200, 400};
    public float[] dimensions_joe = new float[]{225, 400};
    public float[] dimensions_mark = new float[]{225,350};
    public float[] dimensions_maverick = new float[]{200,350};
    public float[] dimensions_nicki = new float[]{350,350};
    public float[] dimensions_grandma = new float[]{350,400};



    private Dictionary<string,int> mapMoodToIndex =
        new Dictionary<string,int>{{"neutral", 0}, {"pleased", 1}, {"upset", 2}};


    //Need to initialize this in Start() since it uses editor-assigned sprite lists
    private Dictionary<string,Sprite[]> mapNameToSprites;
    private Dictionary<string,float[]> mapNameToDimensions;

    public event Action<string> SetNextStateEvent;

    void Awake()
    {
        //Needs to be on Awake() instead of Start()
        //  I think(?) because the sprite references in these arrays are editor-assigned
        mapNameToSprites = new Dictionary<string, Sprite[]>{
            {"Saint Casey", npcDialogIcons_casey},
            {"Casey The Heretic", npcDialogIcons_casey},
            {"Sir Jonathan", npcDialogIcons_joe},
            {"Mark The Bard", npcDialogIcons_mark},
            {"Maverick The Monk", npcDialogIcons_maverick},
            {"Nyx", npcDialogIcons_nicki},
            {"Grandma Yaga", npcDialogIcons_grandma}
        };

        mapNameToDimensions = new Dictionary<string, float[]>{
            {"Saint Casey", dimensions_casey},
            {"Casey The Heretic", dimensions_casey},
            {"Sir Jonathan", dimensions_joe},
            {"Mark The Bard", dimensions_mark},
            {"Maverick The Monk", dimensions_maverick},
            {"Nyx", dimensions_nicki},
            {"Grandma Yaga", dimensions_grandma}
        };

        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    public void initializeConversation(string npcSpeaker) {
        resetTextElements();

        playerNameplate.text = playerName;
        playerNameplate.enabled = true;
        activePlayerImage.enabled = true;

        Debug.Log("zstarting with " + npcSpeaker);
        string speakerDisplayName = turnCodeNameIntoDisplayName(npcSpeaker);
        Debug.Log("zzstarting with " + speakerDisplayName);
        npcNameplate.text = speakerDisplayName;
        npcNameplate.enabled = true;

        //Find the correct list of NPC sprites to pull from, and then
        //  use the one whose mood matches the supplied _playerMood
        //Default to neutral 
        activeNPCImage.sprite = mapNameToSprites[speakerDisplayName][mapMoodToIndex[DEFAULT_MOOD]];
        activeNPCImage.GetComponent<RectTransform>().sizeDelta = new Vector2(
            mapNameToDimensions[speakerDisplayName][0],
            mapNameToDimensions[speakerDisplayName][1]
        );
        activeNPCImage.enabled = true;
    }

    //Update the main text body
    private void speakStatement(Conversation_Statement _statement) {
        resetTextElements();

        activePlayerImage.enabled = true;
        activeNPCImage.enabled = true;

        playerNameplate.enabled = true;
        npcNameplate.enabled = true;

        speechText.text = _statement.text;
    }

    //Display all of the elements to let a player choose their next dialog option
    public void playerIsChoosing(Conversation_Transition _transition) {
        resetTextElements();

        if (_transition.options.Length > playerTextButtonBoxes.Length) {
            Debug.LogError("More dialog options available than can be displayed");
        } else if (_transition.options.Length == 0) {
            Debug.LogError("No conversation options available");
        } else {
            Debug.Log("Player has options to choose from: " + _transition.options);

            for (int i = 0; i < _transition.options.Length; i++) {
                //Add a reference on the button to the text (needed for onClick functionality)
                playerTextButtonBoxes[i].GetComponent<OptionHolder>().option = _transition.options[i];
                playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = _transition.options[i].optionText;
                playerTextButtonBoxes[i].gameObject.SetActive(true);
            }
        }

        //Disable all the icons while options are displayed
        activePlayerImage.enabled = false;
        activeNPCImage.enabled = false;

        playerNameplate.enabled = false;
        npcNameplate.enabled = false;
    }


    //Player has chosen a dialog option, so show them speaking it
    public void playerStatement(Conversation_Statement _playerStatement) {
        activePlayerImage.color = new Color(activePlayerImage.color.r, activePlayerImage.color.g, activePlayerImage.color.b, 1f);
        activeNPCImage.color = new Color(activeNPCImage.color.r, activeNPCImage.color.g, activeNPCImage.color.b, 0.5f);
        speakStatement(_playerStatement);
    }

    //NPC is speaking, so show them speaking it
    public void npcStatement(Conversation_Statement _npcStatement) {
        activePlayerImage.color = new Color(activePlayerImage.color.r, activePlayerImage.color.g, activePlayerImage.color.b, 0.5f);
        activeNPCImage.color = new Color(activeNPCImage.color.r, activeNPCImage.color.g, activeNPCImage.color.b, 1f);
        npcNameplate.text = turnCodeNameIntoDisplayName(_npcStatement.speaker);
        speakStatement(_npcStatement);
    }

    public void resetTextElements() {
        //Deletes the text contents of all text elements of the Dialog Window.
        //Also sets the buttons to inactive so that "empty" dialog options don't show up
        for (int i = 0; i < playerTextButtonBoxes.Length; i++) {
            playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = "";
            playerTextButtonBoxes[i].gameObject.SetActive(false);
        }

        speechText.text = "";
    }

    public string turnCodeNameIntoDisplayName(string _codeName) {
        return _codeName.Replace("_", " ");
    }

    public void onOptionButtonClicked(int _index) {
        Debug.Log("Button " + _index + " clicked");
        Debug.Log("Now we should go to state "+playerTextButtonBoxes[_index].GetComponent<OptionHolder>().option.target);

        SetNextStateEvent?.Invoke(playerTextButtonBoxes[_index].GetComponent<OptionHolder>().option.target);
    }
}
