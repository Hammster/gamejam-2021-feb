using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Put this on any object on your scene
public class DevEnviromentLoader : MonoBehaviour
{
    void Awake()
    {
        GameObject check = GameObject.Find("__app");
        if (check == null) 
        { 
            var loadedScene = SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene("_preload");
            UnityEngine.SceneManagement.SceneManager.LoadScene(loadedScene);
        }
    }
}