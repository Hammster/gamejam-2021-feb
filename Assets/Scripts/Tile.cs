using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    public static int TILE_SIZE = 4;
    public BoardManager boardManager;

    public void MoveTile(int xNew, int yNew, float animationTime, System.Action finalAction) {
        xIndex = xNew;
        yIndex = yNew;
        if (finalAction == null) {
            LeanTween.moveLocal(gameObject, new Vector3(xNew*TILE_SIZE, yNew*TILE_SIZE, 0), animationTime).setEaseInOutExpo();
        } else {
            LeanTween.moveLocal(gameObject, new Vector3(xNew*TILE_SIZE, yNew*TILE_SIZE, 0), animationTime).setEaseInOutExpo().setOnComplete(finalAction);
        }
    }

    public TileScriptableObject GetTileType() {
        return gameObject.GetComponent<TileController>().tileSource;
    }

    void OnMouseDown() {
        if (boardManager == null) {
            boardManager = Reference.GetInstance().boardManager;
        }
        boardManager.ClickedTile(this);
    }
}
