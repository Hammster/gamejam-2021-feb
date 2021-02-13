using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimation : MonoBehaviour
{
    public float forceDir = .2f;
    public float forceRotation = 6f;
    public float secondsToDir = 10f;
    public float secondsToRotation = 7f;

    public bool isEnemy = false; 
    
    void Awake()
    {
        if(isEnemy) 
        {
            Appoach();
        } 
        else 
        {
            Idle();
        }
    }

    void Idle() 
    {
        LeanTween.moveLocalY(gameObject, gameObject.transform.position.y - forceDir, secondsToDir).setLoopPingPong();
        LeanTween.rotateZ(gameObject, gameObject.transform.rotation.z + forceRotation, secondsToRotation).setLoopPingPong();
    }

    void Sink() 
    {
        LeanTween.moveLocalY(gameObject, -5f, 2);
    }

    void Appoach() 
    {
        gameObject.transform.position.Set(-6f, gameObject.transform.position.y, gameObject.transform.position.z);
        LeanTween.moveLocalX(gameObject, 1.629f, 3);
    }
}
