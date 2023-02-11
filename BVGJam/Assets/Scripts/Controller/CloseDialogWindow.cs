using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseDialogWindow : MonoBehaviour {

    void Awake() {
    }

    public void dialogClose() {
        //TODO do we need this now that we have the registered events in DialogManager?
        //Probably this could all be handled in dialogmanager
        DialogManager.instance.dialogOpen = false;
        SceneManager.UnloadSceneAsync("DialogWindow");
    }

    public void dramaticDialogClose() {
        DialogManager.instance.dialogOpen = false;
        SceneManager.UnloadSceneAsync("DramaticDialogWindow");
    }
}
