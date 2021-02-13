using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int boardWidth = 10;
    public int boardHeight = 8;
    public float tileSize = 10.0f;
    public GameObject tilePrefab;// = Resources.Load("Prefabs/Headquarter", typeof(GameObject));
    public void CreateBoard(List<TileScriptableObject> availableTileTypes)
    {
        GameObject[,] board = new GameObject[boardWidth,boardHeight];
        for (int x = 0; x <= boardWidth; x++) {
            for (int y = 0; y <= boardHeight; y++) {
                float xPos = x * tileSize;
                float yPos = y * tileSize;
                board[x, y] = MonoBehaviour.Instantiate(tilePrefab, new Vector3(xPos, yPos, 0.0f), Quaternion.identity);

            }
        }
    }

    
}