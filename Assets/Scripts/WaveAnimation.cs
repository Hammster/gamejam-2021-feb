using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimation : MonoBehaviour
{
    public float forceDir;
    public float secondsOfAnimation = 10;

    void Awake()
    {
        LeanTween.moveLocalX(gameObject, gameObject.transform.position.x - forceDir, secondsOfAnimation).setLoopPingPong();
    }
}
