﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DialogData_Old {

    private static Conversation activeConversation;
    private static Dictionary<string, string> parameters;

    public static void load(string _sceneName, Dictionary<string, string> _parameters = null) {
        DialogData_Old.parameters = _parameters;
        
        if (_sceneName == "DialogWindow" || _sceneName == "DramaticDialogWindow") {
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Additive);
        }
        else{
            SceneManager.LoadScene(_sceneName);
        }
    }

    public static void load(string _sceneName, string _key, string _val) {
        DialogData_Old.parameters = new Dictionary<string, string>();
        DialogData_Old.parameters.Add(_key, _val);

        if (_sceneName == "DialogWindow" || _sceneName == "DramaticDialogWindow") {
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Additive);
        }
        else{
            SceneManager.LoadScene(_sceneName);
        }
    }

    public static Dictionary<string, string> getParameters() {
        return parameters;
    }

    public static string getParam(string _key) {
        if (parameters == null) {
            return "";
        }
        return parameters[_key];
    }

    public static void setParam(string _key, string _val) {
        if (parameters == null) {
            DialogData_Old.parameters = new Dictionary<string, string>();
        }
        DialogData_Old.parameters.Add(_key, _val);
    }
}
