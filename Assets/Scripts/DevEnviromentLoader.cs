using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Put this on any object on your scene
public class DevEnviromentLoader : MonoBehaviour
{
    void Awake()
    {
        var app = GameObject.Find("__app");
        if (app == null) 
        { 
            var loadedScene = SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene("_preload");
            UnityEngine.SceneManagement.SceneManager.LoadScene(loadedScene);
        }
    }
}