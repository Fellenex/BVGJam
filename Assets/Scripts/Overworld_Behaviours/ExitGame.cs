using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {
    void Update() {
        //Todo make more robust
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }
}
