using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int boardWidth = 10;
    public int boardHeight = 8;
    public float tileSize = 10.0f;
    public GameObject tilePrefab;
    public List<TileScriptableObject> availableTileTypes;
    private GameObject[,] board;
    
    public void Awake() {
        CreateBoard();
    }
    public void CreateBoard()
    {
        // the game board with the lowest left tile being 0,0
        board = new GameObject[boardWidth,boardHeight];
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                float xPos = x * tileSize;
                float yPos = y * tileSize;
                
                // create the actual tile element in the scene and store it
                GameObject createdTile = MonoBehaviour.Instantiate(tilePrefab, new Vector3(xPos, yPos, 0.0f), Quaternion.identity);
                board[x, y] = createdTile;

                // set the tile's tile type
                int rndIndex = Random.Range(0, availableTileTypes.Count);
                TileScriptableObject tileType = availableTileTypes[rndIndex];
                var tileController = createdTile.GetComponent<TileController>();
                tileController.tileSource = tileType;
                tileController.Inititalize();
            }
        }
    }

    /**
    Checks the board for any matches and removes matching tiles, triggers falling of tiles with empty tiles below and ensures that at least one possible match is available.
    */
    public void CheckBoard() {
        List<Vector2> allMatches = new List<Vector2>();
        // check for matches
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                if (allMatches.Contains(new Vector2(x,y))) {
                    continue;
                }
                List<Vector2> matches = FindMatches(x, y);
                foreach (Vector2 tile in matches) {
                    allMatches.Add(tile);
                }
            }
        }
        
        if (allMatches.Count > 0) {
            // TODO: matches Tiles would have an effect (increase score, add items, move ship, etc)
            //TriggerMatch(allMatches);

            RemoveTiles(allMatches);
            ApplyGravity();
            // TODO: add new cells on top to refill board
            //AddNewCells();
        }
        // ensure at least 1 possible match exists
    }

    void ApplyGravity() {
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {

            }
        }
    }

    void RemoveTiles(List<Vector2> tiles) {
        foreach (Vector2 tile in tiles) {
            int x = (int) tile.x;
            int y = (int) tile.y;

            GameObject tileObject = board[x, y];
            Destroy(tileObject);
            board[x, y] = null;
        }
    }

    List<Vector2> FindMatches(int x, int y) {
        List<Vector2> potentialHorizontalMatches = new List<Vector2>();
        List<Vector2> potentialVerticalMatches = new List<Vector2>();
        TileScriptableObject type = board[x,y].GetComponent<TileController>().tileSource;
                
        // look horizontally to the left
        for (int i = x-1; i >= 0; i--) {
            TileScriptableObject neighbourType = board[i,y].GetComponent<TileController>().tileSource;
            if (neighbourType.Equals(type)) {
                potentialHorizontalMatches.Add(new Vector2(i,y));
            } else {
                break;
            }
        }

        // look horizontally to the left
        for (int i = x+1; i >= 0; i++) {
            TileScriptableObject neighbourType = board[i,y].GetComponent<TileController>().tileSource;
            if (neighbourType.Equals(type)) {
                potentialHorizontalMatches.Add(new Vector2(i,y));
            } else {
                break;
            }
        }

        // look vertically below
        for (int i = x-1; i >= 0; i--) {
            TileScriptableObject neighbourType = board[i,y].GetComponent<TileController>().tileSource;
            if (neighbourType.Equals(type)) {
                potentialVerticalMatches.Add(new Vector2(i,y));
            } else {
                break;
            }
        }

        // look vertically above
        for (int i = x+1; i >= 0; i++) {
            TileScriptableObject neighbourType = board[i,y].GetComponent<TileController>().tileSource;
            if (neighbourType.Equals(type)) {
                potentialVerticalMatches.Add(new Vector2(i,y));
            } else {
                break;
            }
        }

        List<Vector2> actualMatches = new List<Vector2>();
        if (potentialHorizontalMatches.Count >= 2) {
            foreach (Vector2 neighbour in potentialHorizontalMatches) {
                actualMatches.Add(neighbour);
            }
        }
        if (potentialVerticalMatches.Count >= 2) {
            foreach (Vector2 neighbour in potentialVerticalMatches) {
                actualMatches.Add(neighbour);
            }
        }
        if (actualMatches.Count > 0) {
            actualMatches.Add(new Vector2(x,y));
        }

        return actualMatches;
    }
}