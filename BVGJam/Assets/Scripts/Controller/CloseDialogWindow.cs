using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseDialogWindow : MonoBehaviour {
    
    private GameObject playerReference;

    void Start() {
        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    public void dialogClose() {
        playerReference.GetComponent<Player>().dialogOpen = false;
        SceneManager.UnloadSceneAsync("DialogWindow");
    }
}
