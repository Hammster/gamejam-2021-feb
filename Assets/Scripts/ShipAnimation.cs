using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimation : MonoBehaviour
{
    public float forceDir = .2f;
    public float forceRotation = 6f;
    public float secondsToDir = 10f;
    public float secondsToRotation = 7f;

    void Awake()
    {
        LeanTween.moveLocalY(gameObject, gameObject.transform.position.y - forceDir, secondsToDir).setLoopPingPong();
        LeanTween.rotateZ(gameObject, gameObject.transform.rotation.z + forceRotation, secondsToRotation).setLoopPingPong();
    }
}
