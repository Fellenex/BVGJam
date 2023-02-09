using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDramaticDialog : MonoBehaviour {

    private string dramaticDialogSceneName = "DramaticDialogWindow";
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Dictionary<string, string> dData = new Dictionary<string,string>();

            //TODO real data
            dData.Add("id", "blue_1");
            dData.Add("colour", "blue");
            dData.Add("text", "Test text");
            //jsonDict.Add("display_name", behaviour.displayName);
            DialogData.load(dramaticDialogSceneName, dData);
        }
    }

    public void createWindow() {
        Dictionary<string, string> dData = new Dictionary<string,string>();
        //TODO real data
        dData.Add("id", "blue_1");
        dData.Add("colour", "blue");
        dData.Add("text", "Test text");
        DialogData.load(dramaticDialogSceneName, dData);
    }
}
