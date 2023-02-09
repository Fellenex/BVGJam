using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour {

    public float delay = 0.08f;
    public string fullText;

    void Start() {
        fullText = GetComponent<Text>().text;
    }

    public void typewriter() {
        StartCoroutine(ShowText());
    }

    public IEnumerator ShowText() {
        string currentText = "";
        for (int i=0; i < fullText.Length; i++) {
            currentText = fullText.Substring(0,i);
            this.GetComponent<Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}
