using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DramaticConversationGraphics : MonoBehaviour {

    public GameObject panel;
    public Text dramaticTextHolder;
    public CanvasRenderer canvas;

    private float timePerFadeIn = 0.1f;
    private float fadeTimeCounter = 0.1f;
    
    //Alpha is measured from 0 to 1 so we want really small increments
    private float fadeSpeed = 0.04f;

    public bool playerCanMoveOn;

    public static Dictionary<ValueTuple<String, String>,Color> DramaticDialogColours = new System.Collections.Generic.Dictionary<ValueTuple<String, String>,Color>{
        {("red", "good"), new Color(248,25,0,255)},
        {("red", "bad"), new Color(199,120,120,255)},
        {("blue", "good"), new Color(11,3,252,255)},
        {("blue", "bad"), new Color(148,183,194,255)},
        {("yellow", "good"), new Color(255,255,51,255)},
        {("yellow", "bad"), new Color(219,224,182,255)},
        {("purple", "good"), new Color(177,0,231,255)},
        {("purple", "bad"), new Color(99,33,133,255)},
        {("green", "good"), new Color(0,232,31,255)},
        {("green", "bad"), new Color(209,237,119,255)}
    };

    public static Color getColorByTrigger(Conversation_Trigger _trigger) {
        return DramaticDialogColours[(_trigger.colour, _trigger.quality)];
    }

    void Start() {
        //Don't let the player skip these scenes until things have properly faded in
        playerCanMoveOn = false;
    

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
