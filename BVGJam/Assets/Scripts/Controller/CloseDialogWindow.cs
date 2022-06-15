﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseDialogWindow : MonoBehaviour {
    
    private GameObject playerReference;

    void Awake() {
        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    public void dialogClose() {
        playerReference.GetComponent<Player>().dialogOpen = false;
        SceneManager.UnloadSceneAsync("DialogWindow");
    }

    public void dramaticDialogClose() {
        playerReference.GetComponent<Player>().dialogOpen = false;
        SceneManager.UnloadSceneAsync("DramaticDialogWindow");
    }
}
