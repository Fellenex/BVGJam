using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public abstract class DialogGraphics : MonoBehaviour {
    private const string PLAYER_NAME = "Pal";
    private const string DEFAULT_MOOD = "neutral";

    private static Color FULL_VISIBILITY = new Color(1f, 1f, 1f, 1f);
    private static Color HALF_VISIBILITY = new Color(1f, 1f, 1f, 0.5f);

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
    

    //Handle activation/deactivation here so that the logic can be separate from DialogController
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
            foreach (Conversation_Option option in _options) {
                Debug.Log(option.optionText);
            }

            //Fill up the option buttons with the Conversation_Options we've been given
            for (int i = 0; i < _options.Count; i++) {
                //Tell the button what Conversation_Option object it's holding
                playerTextButtonBoxes[i].GetComponent<OptionHolder>().option = _options[i];

                //Display the text contained in that Conversation_Option
                playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = _options[i].optionText;

                /*
                Add the event listener to handle the player clicking this button
                See https://stackoverflow.com/questions/271440/captured-variable-in-a-loop-in-c-sharp for this closure index magic
                */
                int closureIndex = i+1;
                playerTextButtonBoxes[i].GetComponent<Button>().onClick.AddListener(() => onOptionButtonClicked(closureIndex));
                playerTextButtonBoxes[i].gameObject.SetActive(true);
            }
        }
    }

    private void onOptionButtonClicked(int _index) {
        Debug.Log("Clicked index "+_index);
        SetNextStateEvent?.Invoke(getOptionFromButton(_index));
    }

    //A small helper function to get a Conversation_Option object from one of the buttons we've setup
    private Conversation_Option getOptionFromButton(int _buttonIndex) {
        return playerTextButtonBoxes[_buttonIndex].GetComponent<OptionHolder>().option;
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

    //Speakerless statement - fades out both the player and the NPC
    public void speakerlessStatement(Conversation_Statement _statement) {
        //Update the character image fading
        activePlayerImage.color = HALF_VISIBILITY;
        activeNPCImage.color = HALF_VISIBILITY;

        speechText.text = _statement.text;
    }

    //Reset the colour back to normal, in case there was a colour trigger before
    //Used at the beginning of a new state, rather than a new statement, but
    //  DialogGraphics doesn't need to know about states.
    public void resetBackgroundColour() {
        childPanel.GetComponent<Image>().color = FULL_VISIBILITY;
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

    protected void resetTextElements() {
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

    public abstract void HandleTrigger(Conversation_Trigger _trigger);

    /*
    //How many seconds to wait for the player to read each character of text
    public float secondsPerCharacterToWait = 5f;
    IEnumerator StatementWait() {
        yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.E));
    }
    */
}
