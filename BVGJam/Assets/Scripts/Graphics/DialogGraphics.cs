using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogGraphics : MonoBehaviour {
    public static DialogGraphics instance;

    private static string PLAYER_NAME = "Pal";

    private static string DEFAULT_MOOD = "neutral";

    public Text playerNameplate;
    public Text npcNameplate;
    public Image activePlayerImage;
    public Image activeNPCImage;

    public Button[] playerTextButtonBoxes;

    public Text speechText;

    public CharacterDisplay playerDisplay;
    public CharacterDisplay npcDisplay;

    public event Action<Conversation_Option> SetNextStateEvent;

    void Awake() {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    public void initializeConversation(string npcSpeaker) {
        resetTextElements();

        Debug.Log("Initializing DialogGraphics attributes");

        //Update player-side nameplate and image
        playerDisplay = CharacterDisplay.ConstructByName(PLAYER_NAME);
        playerNameplate.text = PLAYER_NAME;
        playerNameplate.enabled = true;
        activePlayerImage.sprite = playerDisplay.images[DEFAULT_MOOD];
        //activePlayerImage.GetComponent<RectTransform>().sizeDelta = playerDisplay.dimensions;
        activePlayerImage.enabled = true;
        
        //Update npc-side nameplate and image
        npcDisplay = CharacterDisplay.ConstructByName(npcSpeaker);
        npcNameplate.text = npcDisplay.displayName;
        npcNameplate.enabled = true;
        activeNPCImage.sprite = npcDisplay.images[DEFAULT_MOOD];
        activeNPCImage.GetComponent<RectTransform>().sizeDelta = npcDisplay.dimensions;
        activeNPCImage.enabled = true;
    }

    //Display all of the elements to let a player choose their next dialog option
    //TODO soon(ish). Should take a list of options, not a transition itself
    public void playerIsChoosing(List<Conversation_Option> _options) {
        resetTextElements();

        if (_options.Count > playerTextButtonBoxes.Length) {
            Debug.LogError("More dialog options available than can be displayed");
        } else if (_options.Count == 0) {
            Debug.LogError("No conversation options available");
        } else {
            Debug.Log("Player has options to choose from: " + _options);

            //TODO does not consider conditions of specific options. Currently all options are just shown.
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
        //Update the fading
        activePlayerImage.color = new Color(activePlayerImage.color.r, activePlayerImage.color.g, activePlayerImage.color.b, 1f);
        activeNPCImage.color = new Color(activeNPCImage.color.r, activeNPCImage.color.g, activeNPCImage.color.b, 0.5f);

        //Update the player's image
        //Assumes the left speaker is always the player
        activePlayerImage.sprite = playerDisplay.images[_playerStatement.mood];
        speakStatement(_playerStatement);
    }

    //NPC is speaking, so show them speaking it
    public void npcStatement(Conversation_Statement _npcStatement) {
        //Update the fading
        activePlayerImage.color = new Color(activePlayerImage.color.r, activePlayerImage.color.g, activePlayerImage.color.b, 0.5f);
        activeNPCImage.color = new Color(activeNPCImage.color.r, activeNPCImage.color.g, activeNPCImage.color.b, 1f);

        //Update the NPC's image and nameplate
        npcNameplate.text = npcDisplay.displayName;
        activeNPCImage.sprite = npcDisplay.images[_npcStatement.mood];
        speakStatement(_npcStatement);
    }

    public void onOptionButtonClicked(int _index) {
        Debug.Log("Button " + _index + " clicked");
        Debug.Log("Now we should go to state "+playerTextButtonBoxes[_index].GetComponent<OptionHolder>().option.target);

        SetNextStateEvent?.Invoke(getOptionFromButton(_index));
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

    private void resetTextElements() {
        //Disable all the character art + nameplates
        activePlayerImage.enabled = false;
        activeNPCImage.enabled = false;
        playerNameplate.enabled = false;
        npcNameplate.enabled = false;

        //Deletes the text contents of all text elements of the Dialog Window.
        //Also sets the buttons to inactive so that "empty" dialog options don't show up
        for (int i = 0; i < playerTextButtonBoxes.Length; i++) {
            playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = "";
            playerTextButtonBoxes[i].gameObject.SetActive(false);
        }

        speechText.text = "";
    }

    private Conversation_Option getOptionFromButton(int _buttonIndex) {
        return playerTextButtonBoxes[_buttonIndex].GetComponent<OptionHolder>().option;
    }

    /*
    //How many seconds to wait for the player to read each character of text
    public float secondsPerCharacterToWait = 5f;
    IEnumerator StatementWait() {
        yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.E));
    }
    */
}
