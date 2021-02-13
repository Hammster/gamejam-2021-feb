using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public TileScriptableObject tileSource;

    void Awake()
    {
        if(tileSource != null) Inititalize();
    }

    public void Inititalize() {
        var sr = gameObject.transform.Find("Icon").GetComponent<SpriteRenderer>();
        sr.sprite = tileSource.sprite;
    }
}
