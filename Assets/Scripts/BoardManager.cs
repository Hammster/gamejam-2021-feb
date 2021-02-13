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
    private bool pressed = false;
    
    public void Awake() {
        CreateBoard("abcabc");
    }
    public void CreateBoard(string seed)
    {
        Random.InitState(seed.GetHashCode());
        // the game board with the lowest left tile being 0,0
        board = new GameObject[boardWidth,boardHeight];
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                SpawnTile(x, y);
            }
        }
    }

    void SpawnTile(int x, int y) {
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

    void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            if (!pressed) {
                pressed = true;
                FindMatches(0, 2);
            }
        } else {
            pressed = false;
        }
    }

    /**
    Checks the board for any matches and removes matching tiles, triggers falling of tiles with empty tiles below and ensures that at least one possible match is available.
    */
    public void CheckBoard() {
        Debug.Log("Checking board!");
        List<Vector2> allMatches = new List<Vector2>();
        // check for matches
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                if (allMatches.Contains(new Vector2(x,y))) {
                    Debug.Log("skipped x: "+ x + ", y: "+ y);
                    continue;
                }
                List<Vector2> matches = FindMatches(x, y);
                foreach (Vector2 tile in matches) {
                    allMatches.Add(tile);
                }
                Debug.Log("added "+matches.Count+" matches for x: "+ x + ", y: "+ y + "(tileType: "+board[x, y].GetComponent<TileController>().tileSource+")");
            }
        }
        
        /*if (allMatches.Count > 0) {
            // TODO: matches Tiles would have an effect (increase score, add items, move ship, etc)
            //TriggerMatch(allMatches);

            RemoveTiles(allMatches);
            
            // empty tiles move down
            ApplyGravity();
            
            // add new cells to refill board
            AddNewCells();
        }*/
        // ensure at least 1 possible match exists
    }

    void AddNewCells() {
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                if (board[x,y] == null) {
                    SpawnTile(x, y);
                }
            }
        }
    }

    void ApplyGravity() {
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                while(board[x,y] == null) {
                    for (int current = y; current < boardHeight; current++) {
                        if (current+1 >= boardHeight) {
                            board[x,current] = null;
                        } else {
                            board[x,current] = board[x,current+1];
                        }
                    }
                }
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

    bool IsMatch(int x, int y, string technicalName, List<Vector2> list) {
        TileScriptableObject neighbourType = board[x,y].GetComponent<TileController>().tileSource;
        Debug.Log("Neighbour type: "+neighbourType);
        if (neighbourType.technicalName.Equals(technicalName)) {
            list.Add(new Vector2(x,y));
            return true;
        }
        return false;
    }

    List<Vector2> FindMatches(int x, int y) {
        Debug.Log("Starting for x: "+ x + ", y: "+ y + "(tileType: "+board[x, y].GetComponent<TileController>().tileSource+")");
        List<Vector2> potentialHorizontalMatches = new List<Vector2>();
        List<Vector2> potentialVerticalMatches = new List<Vector2>();
        TileScriptableObject type = board[x,y].GetComponent<TileController>().tileSource;

        // look horizontally to the left
        for (int i = x-1; i >= 0; i--) {
            Debug.Log("left i="+i);
            if (!IsMatch(i, y, type.technicalName, potentialHorizontalMatches)) {
                break;
            }
        }

        // look horizontally to the right
        for (int i = x+1; i < boardWidth; i++) {
            Debug.Log("right i="+i);
            if (!IsMatch(i, y, type.technicalName, potentialHorizontalMatches)) {
                break;
            }
        }

        // look vertically below
        for (int i = y-1; i >= 0; i--) {
            Debug.Log("down i="+i);
            if (!IsMatch(x, i, type.technicalName, potentialVerticalMatches)) {
                break;
            }
        }

        // look vertically above
        for (int i = y+1; i < boardWidth; i++) {
            Debug.Log("up i="+i);
            if (!IsMatch(x, i, type.technicalName, potentialVerticalMatches)) {
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