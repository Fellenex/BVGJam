using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogGraphics : MonoBehaviour {
    private static string PLAYER_NAME = "Pal";

    private static string DEFAULT_MOOD = "neutral";

    private static Color FULL_VISIBILITY = new Color(1f, 1f, 1f, 1f);
    private static Color HALF_VISIBILITY = new Color(1f, 1f, 1f, 0.5f);

    //Apparently we have to set Colors with normalized (0-1) values if it's editing an Image from a script
    //Problem: https://issuetracker.unity3d.com/issues/image-color-cannot-be-changed-via-script-when-image-type-is-set-to-simple
    //Solution: https://forum.unity.com/threads/ui-image-color-not-updating-correctly.674827/
    private static Dictionary<ValueTuple<String, String>,Color> TRIGGER_COLOURS =
        new System.Collections.Generic.Dictionary<ValueTuple<String, String>,Color>{
        {("red", "good"),       new Color(248f/255f,    25f/255f,   0f,         1f)},
        {("red", "bad"),        new Color(199f/255f,    120f/255f,  120f/255f,  1f)},
        {("blue", "good"),      new Color(11f/255f,     3f/255f,    252f/255f,  1f)},
        {("blue", "bad"),       new Color(148f/255f,    183f/255f,  194f/255f,  1f)},
        {("yellow", "good"),    new Color(255f/255f,    255f/255f,  51f/255f,   1f)},
        {("yellow", "bad"),     new Color(219f/255f,    224f/255f,  182f/255f,  1f)},
        {("purple", "good"),    new Color(177f/255f,    0f,         231f/255f,  1f)},
        {("purple", "bad"),     new Color(99f/255f,     33f/255f,   133f/255f,  1f)},
        {("green", "good"),     new Color(0f,           232f/255f,  31f/255f,   1f)},
        {("green", "bad"),      new Color(209f/255f,    237f/255f,  119f/255f,  1f)}
    };

    public Text playerNameplate;
    public Text npcNameplate;
    public Image activePlayerImage;
    public Image activeNPCImage;

    public Button[] playerTextButtonBoxes;

    public Text speechText;

    //Canvas with DialogGraphics should only have one child panel
    public GameObject childPanel;

    public CharacterDisplay playerDisplay;
    public CharacterDisplay npcDisplay;

    public event Action<Conversation_Option> SetNextStateEvent;
    

    //Handle activation/deactivation here so that the logic can be separate fromm DialogController
    public void activate() { childPanel.gameObject.SetActive(true); }
    public void deactivate() { childPanel.gameObject.SetActive(false); }

    public void initializeConversation(string npcSpeaker) {
        resetTextElements();
        resetBackgroundColour();

        //Update player-side nameplate and image
        playerDisplay = CharacterDisplay.ConstructByName(PLAYER_NAME);
        playerNameplate.text = PLAYER_NAME;
        playerNameplate.enabled = true;
        activePlayerImage.sprite = playerDisplay.images[DEFAULT_MOOD];
        activePlayerImage.GetComponent<RectTransform>().sizeDelta = playerDisplay.dimensions;
        activePlayerImage.enabled = true;
        
        //Update npc-side nameplate and image
        npcDisplay = CharacterDisplay.ConstructByName(npcSpeaker);
        npcNameplate.text = npcDisplay.displayName;
        npcNameplate.enabled = true;
        activeNPCImage.sprite = npcDisplay.images[DEFAULT_MOOD];
        activeNPCImage.GetComponent<RectTransform>().sizeDelta = npcDisplay.dimensions;
        activeNPCImage.enabled = true;
    }

    //Display all of the player's elements to let a player choose their next dialog option
    public void playerIsChoosing(List<Conversation_Option> _options) {
        resetTextElements();

        if (_options.Count > playerTextButtonBoxes.Length) {
            Debug.LogError("More dialog options available than can be displayed");
        } else if (_options.Count == 0) {
            Debug.LogError("No conversation options available");
        } else {
            Debug.Log("Player has options to choose from: " + _options);

            //Fill up the option buttons with the options we've been given
            for (int i = 0; i < _options.Count; i++) {
                //Add a reference on the button to the text (needed for onClick functionality)
                playerTextButtonBoxes[i].GetComponent<OptionHolder>().option = _options[i];
                playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = _options[i].optionText;
                playerTextButtonBoxes[i].gameObject.SetActive(true);
            }
        }
    }

    //Player has chosen a dialog option, so show them speaking it
    public void playerStatement(Conversation_Statement _playerStatement) {
        //Update the character image fading
        activePlayerImage.color = FULL_VISIBILITY;
        activeNPCImage.color = HALF_VISIBILITY;

        //Update the player's image
        //Assumes the left speaker is always the player
        activePlayerImage.sprite = playerDisplay.images[_playerStatement.mood];
        speakStatement(_playerStatement);
    }

    //NPC is speaking, so show them speaking it
    public void npcStatement(Conversation_Statement _npcStatement) {
        //Update the character image fading
        activePlayerImage.color = HALF_VISIBILITY;
        activeNPCImage.color = FULL_VISIBILITY;

        //The NPC may have changed in-between states or statements.
        npcDisplay = CharacterDisplay.ConstructByName(_npcStatement.speaker);

        //Update the NPC's image and nameplate
        Debug.Log("About to show " + npcDisplay.displayName + " in a " + _npcStatement.mood + " mood");
        npcNameplate.text = npcDisplay.displayName;
        activeNPCImage.sprite = npcDisplay.images[_npcStatement.mood];
        speakStatement(_npcStatement);
    }

    //Speakerless statement - for use in special colour trigger states
    public void speakerlessStatement(Conversation_Statement _statement) {
        //Update the character image fading
        activePlayerImage.color = HALF_VISIBILITY;
        activeNPCImage.color = HALF_VISIBILITY;

        speechText.text = _statement.text;
    }

    public void handleTrigger(Conversation_Trigger _trigger) {
        resetTextElements();

        //Update the background's colour to reflect the current colour trigger
        //TODO future - have something more dramatic. Fade-in, not just the default background image, etc
        childPanel.GetComponent<Image>().color = getColorByTrigger(_trigger);
        Debug.Log(childPanel.GetComponent<Image>().color);
    }

    //Reset the colour back to normal, in case there was a colour trigger before
    //Used at the beginning of a new state, rather than a new statement, but
    //  DialogGraphics doesn't need to know about states.
    public void resetBackgroundColour() {
        childPanel.GetComponent<Image>().color = FULL_VISIBILITY;
    }

    //Used as the OnClick function by the buttons created when a player has a choice
    public void onOptionButtonClicked(int _index) {
        Debug.Log("Now we should go to state "+playerTextButtonBoxes[_index].GetComponent<OptionHolder>().option.target);
        SetNextStateEvent?.Invoke(getOptionFromButton(_index));
    }

    //A small helper function to get a Conversation_Option object from one of the buttons we've setup
    private Conversation_Option getOptionFromButton(int _buttonIndex) {
        return playerTextButtonBoxes[_buttonIndex].GetComponent<OptionHolder>().option;
    }

    private static Color getColorByTrigger(Conversation_Trigger _trigger) {
        return TRIGGER_COLOURS[(_trigger.colour, _trigger.quality)];
    }

    //Update the main text body using a new statement
    private void speakStatement(Conversation_Statement _statement) {
        resetTextElements();
        Debug.Log("speakStatement - " + _statement.text);

        activePlayerImage.enabled = true;
        activeNPCImage.enabled = true;
        playerNameplate.enabled = true;
        npcNameplate.enabled = true;

        speechText.text = _statement.text;
    }

    private void resetTextElements() {
        //Disable all the character art + nameplates
        activePlayerImage.enabled = false;
        activeNPCImage.enabled = false;
        playerNameplate.enabled = false;
        npcNameplate.enabled = false;

        //Change the background back to normal
        //childPanel.GetComponent<Image>().color = FULL_VISIBILITY;

        //Deletes the text contents of all text elements of the Dialog Window.
        //Also sets the buttons to inactive so that "empty" dialog options don't show up
        for (int i = 0; i < playerTextButtonBoxes.Length; i++) {
            playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = "";
            playerTextButtonBoxes[i].gameObject.SetActive(false);
        }

        speechText.text = "";
    }

    /*
    //How many seconds to wait for the player to read each character of text
    public float secondsPerCharacterToWait = 5f;
    IEnumerator StatementWait() {
        yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.E));
    }
    */
}
