using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DramaticDialogManager : MonoBehaviour {

    public GameObject panel;
    public Text dramaticTextHolder;
    public CanvasRenderer canvas;

    private float timePerFadeIn = 0.1f;
    private float fadeTimeCounter = 0.1f;
    
    //Alpha is measured from 0 to 1 so we want really small increments
    private float fadeSpeed = 0.04f;

    public bool playerCanMoveOn;

    public Color colorConversion(string _colorName) {
        switch(_colorName) {
            case "red":
                return new Color(255,0,0,0);

            case "green":
                return new Color(0,255,0,0);

            case "blue":
                return new Color(0,0,255,0);

            default:
                return new Color(255,255,255,0);
        }
    }

    void Start() {
        //Don't let the player skip these scenes until things have properly faded in
        playerCanMoveOn = false;
        DialogManager.instance.dialogOpen = true;

        //Reset the canvas to have no alpha and be totally black
        canvas = panel.GetComponent<CanvasRenderer>();
        canvas.SetAlpha(0);
        canvas.SetColor(new Color(0,0,0,0));
    }

    public void Awake() {
        //Set the text up based on parameters passed through DialogData
        //dramaticTextHolder.color = colorConversion(DialogData.getParam("color"));
        //dramaticTextHolder.text = DialogData.getParam("text");
    }

    void Update() {
        //Slowly increase the alpha while the fade timer is on
        
        if (fadeTimeCounter <= 0) {
            //Reset the timer
            fadeTimeCounter = timePerFadeIn;

            //Update the black background's alpha and the white text's alpha to appear more
            //canvas.SetAlpha(canvas.GetAlpha() + fadeSpeed);
            
            Color oldCanvasColor = canvas.GetColor();
            canvas.SetColor(new Color(oldCanvasColor.r, oldCanvasColor.g, oldCanvasColor.b, oldCanvasColor.a + fadeSpeed));

            Color oldTextColor = dramaticTextHolder.color;
            dramaticTextHolder.color = new Color(oldTextColor.r, oldTextColor.g, oldTextColor.b, oldTextColor.a + fadeSpeed);

            Debug.Log("Canvas alpha is now "+canvas.GetAlpha());
            Debug.Log("Text alpha is now "+dramaticTextHolder.color.a);
        }
        else{
            fadeTimeCounter -= Time.deltaTime;
        }

        if (canvas.GetAlpha() >= 1){
            playerCanMoveOn = true;
        }

        checkForPlayerClose();
    }

    void checkForPlayerClose() {
        if (playerCanMoveOn && 
                (Input.GetKeyDown(KeyCode.E)
                || Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.Space)
                || Input.GetKeyDown(KeyCode.KeypadEnter)
                || Input.GetMouseButtonDown(0))) {

            
        }
    }
}
