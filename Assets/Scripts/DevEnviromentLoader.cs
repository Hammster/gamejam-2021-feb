using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put this on any object on your scene
public class DevEnviromentLoader : MonoBehaviour
{
    void Awake()
    {
        GameObject check = GameObject.Find("__app");
        if (check == null) 
        { 
            UnityEngine.SceneManagement.SceneManager.LoadScene("_preload");
        }
    }
}