using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DramaticDialogManager : MonoBehaviour {

    public GameObject panel;
    public Text dramaticTextHolder;
    public CanvasRenderer canvas;

    private float fadeTime = 5f;
    private float fadeSpeed = 1;
    private int fadeCount;

    private Color black;

    void Start() {
        panel.GetComponentInChildren<CanvasRenderer>();
    }

    void Update() {

        //Slowly increase the alpha while the fade timer is on
        if (fadeTime > 0) {
            fadeTime -= Time.deltaTime;
            canvas.SetAlpha(canvas.GetAlpha() + fadeSpeed);
            
            Color oldColor = dramaticTextHolder.color;
            dramaticTextHolder.color = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a + 1);
        }
    }
}
