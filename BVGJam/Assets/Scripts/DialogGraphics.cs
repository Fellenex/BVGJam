using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogGraphics : MonoBehaviour {

    public Text activeSpeakerText;
    public Image activeSpeakerImage;

    public Button[] playerTextButtonBoxes;

    public Text npcText;

    public Sprite[] playerDialogIcons;

    public Sprite[] npcDialogIcons_casey;
    public Sprite[] npcDialogIcons_joe;
    public Sprite[] npcDialogIcons_mark;
    public Sprite[] npcDialogIcons_maverick;
    public Sprite[] npcDialogIcons_nicki;

    private Dictionary<string,int> mapMoodToIndex =
        new Dictionary<string,int>{{"neutral", 0}, {"pleased", 1}, {"upset", 2}};


    //Need to initialize this in Start() since it uses editor-assigned sprite lists
    private Dictionary<string,Sprite[]> mapNameToSprites;

    
    void Awake() {

        //Needs to be on Awake() instead of Start()
        mapNameToSprites = new Dictionary<string,Sprite[]>{
            {"Casey", npcDialogIcons_casey},
            {"Joe", npcDialogIcons_joe},
            {"Mark", npcDialogIcons_mark},
            {"Maverick", npcDialogIcons_maverick},
            {"Nicki", npcDialogIcons_nicki}
        };
    }

    public void playerIsSpeaking(List<Conversation_Transition> _playerTextOptions, string _playerMood) {
        resetTextElements();
        
        Conversation_Transition[] optionArray = _playerTextOptions.ToArray();

        if (optionArray.Length > playerTextButtonBoxes.Length) {
            Debug.LogError("More dialog options available than can be displayed");
        }
        else if (optionArray.Length == 0) {
            Debug.LogError("No conversation options available");
        }
        else{
            for (int i=0; i<optionArray.Length; i++) {
                //Set the text of the current button to match the player's dialog option
                playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = optionArray[i].text;

                //Add a reference on the button to the transition itself (needed for onClick functionality)
                playerTextButtonBoxes[i].GetComponent<TransitionHolder>().transition = optionArray[i];

                //Display the button because there's text available
                playerTextButtonBoxes[i].gameObject.SetActive(true);
            }
            Debug.Log(optionArray);
        }

        activeSpeakerImage.sprite = playerDialogIcons[mapMoodToIndex[_playerMood]];
        activeSpeakerText.text = "Couleur";
    }

    public void npcIsSpeaking(string _speakersName, string _npcText, string _npcMood) {
        resetTextElements();

        //Update the main text body
        npcText.text = _npcText;

        Debug.Log(_speakersName);
        Debug.Log(mapMoodToIndex[_npcMood]);
        Debug.Log(mapNameToSprites[_speakersName]);

        //Find the correct list of NPC sprites to pull from, and then
        //  use the one whose mood matches the supplied _playerMood
        activeSpeakerImage.sprite = mapNameToSprites[_speakersName][mapMoodToIndex[_npcMood]];
        
        //Show the name of the NPC currently speaking
        activeSpeakerText.text = _speakersName;
    }

    public void resetTextElements() {
        //Deletes the text contents of all text elements of the Dialog Window.
        //Also sets the buttons to inactive so that "empty" dialog options don't show up
        for (int i = 0; i < playerTextButtonBoxes.Length; i++) {
            playerTextButtonBoxes[i].GetComponentInChildren<Text>().text = "";
            playerTextButtonBoxes[i].gameObject.SetActive(false);
        }
        npcText.text = "";
        activeSpeakerText.text = "";
    }
}
