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
        var fgsr = gameObject.transform.Find("Icon").GetComponent<SpriteRenderer>();
        var bgsr = gameObject.GetComponent<SpriteRenderer>();
        fgsr.sprite = tileSource.fgSprite;
        bgsr.sprite = tileSource.bgSprite;
    }
}
