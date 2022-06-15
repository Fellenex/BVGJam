using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogGraphics : MonoBehaviour {

    private string playerName = "Pal";

    public Text playerNameplate;
    public Text npcNameplate;
    public Image activePlayerImage;
    public Image activeNPCImage;

    public Button[] playerTextButtonBoxes;

    public Text npcText;
    public Text playerText;

    public Sprite[] playerDialogIcons;
    public Sprite[] npcDialogIcons_casey;
    public Sprite[] npcDialogIcons_joe;
    public Sprite[] npcDialogIcons_mark;
    public Sprite[] npcDialogIcons_maverick;
    public Sprite[] npcDialogIcons_nicki;
    public Sprite[] npcDialogIcons_grandma;

    private Dictionary<string,int> mapMoodToIndex =
        new Dictionary<string,int>{{"neutral", 0}, {"pleased", 1}, {"upset", 2}};


    //Need to initialize this in Start() since it uses editor-assigned sprite lists
    private Dictionary<string,Sprite[]> mapNameToSprites;

    
    void Awake() {
        //Needs to be on Awake() instead of Start()
        //  I think(?) because the sprite references in these arrays are editor-assigned
        mapNameToSprites = new Dictionary<string,Sprite[]>{
            {"Casey", npcDialogIcons_casey},
            {"Sir Jonathan", npcDialogIcons_joe},
            {"Mark", npcDialogIcons_mark},
            {"Maverick", npcDialogIcons_maverick},
            {"Nicki", npcDialogIcons_nicki},
            {"Grandma Yaga", npcDialogIcons_grandma}
        };
    }


    //Display all of the elements to let a player choose their next dialog option
    public void playerIsChoosing(List<Conversation_Transition> _playerTextOptions) {
        resetTextElements();
        
        Conversation_Transition[] optionArray = _playerTextOptions.ToArray();

        if (optionArray.Length > playerTextButtonBoxes.Length) {
            Debug.LogError("More dialog options available than can be displayed");
        }
        else if (optionArray.Length == 0) {
            Debug.LogError("No conversation options available");
        }
        else{
            Debug.Log("Player has options to choose from: " + optionArray);

            for (int i=0; i<optionArray.Length; i++) {
                //Set the text of the current button to match the player's dialog option
                //playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = optionArray[i].statements[0].text;

                //If no option text was supplied, then "fail gracefully" by showing the plain speech text.
                if (optionArray[i].optionText == "") {
                    playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = optionArray[i].statements[0].text;
                }
                else{
                    playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = optionArray[i].optionText;
                }

                //Add a reference on the button to the transition itself (needed for onClick functionality)
                playerTextButtonBoxes[i].GetComponent<TransitionHolder>().transition = optionArray[i];

                //Display the button because there's text available
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
        resetTextElements();

        Debug.Log("Player is saying: "+_playerStatement.text);

        //Update the main text body
        playerText.text = _playerStatement.text;

        //Find the right icon for the player based on their mood
        activeNPCImage.enabled = false;
        activePlayerImage.sprite = playerDialogIcons[mapMoodToIndex[_playerStatement.mood]];
        activePlayerImage.enabled = true;

        //Swap nameplates
        npcNameplate.enabled = false;
        playerNameplate.text = playerName;
        playerNameplate.enabled = true;
    }


    //NPC is speaking, so show them speaking it
    public void npcStatement(Conversation_Statement _npcStatement, string _speaker) {
        resetTextElements();

        Debug.Log("NPC is saying: "+_npcStatement.text);

        //Update the main text body
        npcText.text = _npcStatement.text;

        activePlayerImage.enabled = false;
        //Find the correct list of NPC sprites to pull from, and then
        //  use the one whose mood matches the supplied _playerMood
        activeNPCImage.sprite = mapNameToSprites[_speaker][mapMoodToIndex[_npcStatement.mood]];
        activeNPCImage.enabled = true;

        //Swap nameplates
        npcNameplate.text = _speaker;
        playerNameplate.enabled = false;
        npcNameplate.enabled = true;
    }

    public void resetTextElements() {
        //Deletes the text contents of all text elements of the Dialog Window.
        //Also sets the buttons to inactive so that "empty" dialog options don't show up
        for (int i = 0; i < playerTextButtonBoxes.Length; i++) {
            playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = "";
            playerTextButtonBoxes[i].gameObject.SetActive(false);
        }

        playerText.text = "";
        //playerNameplate.text = "";        //don't need this since the player nameplate doesn't change
        npcText.text = "";
        npcNameplate.text = "";
    }
}
