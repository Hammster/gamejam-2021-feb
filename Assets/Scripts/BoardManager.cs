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
    private Tile[,] board;
    private bool pressed = false;
    private Tile higlightedTile = null;
    private int score = 0;

    public void Awake() {
        CreateBoard("abcabc");
        // Self register at reference
        Reference.GetInstance().boardManager = gameObject.GetComponent<BoardManager>();
    }

    public void CreateBoard(string seed)
    {
        LeanTween.init(500);
        Random.InitState(seed.GetHashCode());
        // the game board with the lowest left tile being 0,0
        board = new Tile[boardWidth,boardHeight];
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                SpawnTile(x, y);
            }
        }
        CheckBoard(false);
    }

    public void ClickedTile(Tile clicked) {
        
        if (higlightedTile == null) {
            higlightedTile = clicked;
            higlightedTile.Highlight(true);
        } else {
            if (!higlightedTile.Equals(clicked)) {
                MakeMove(higlightedTile, clicked);
            }
            higlightedTile.Highlight(false);
            higlightedTile = null;
        }
    } 

    void SpawnTile(int x, int y) {
        float xPos = x * tileSize;
        float yPos = y * tileSize;
        float scalingTime = 0.66f; // in seconds
        
        // create the actual tile element in the scene and store it
        GameObject createdTile = MonoBehaviour.Instantiate(tilePrefab, new Vector3(xPos, yPos, 0.0f), Quaternion.identity);
        createdTile.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(createdTile, new Vector3(4, 4, 4), scalingTime).setEaseInOutExpo();
        Tile newTile = createdTile.GetComponent<Tile>();
        newTile.xIndex = x;
        newTile.yIndex = y;
        board[x, y] = newTile;

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
                //CheckBoard();
            }
        } else {
            pressed = false;
        }
    }

    /**
    Checks the board for any matches and removes matching tiles, triggers falling of tiles with empty tiles below and ensures that at least one possible match is available.
    */
    void CheckBoard(bool countScore = true) {
        List<Vector2> allMatches = new List<Vector2>();
        // check for matches
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                if (allMatches.Contains(new Vector2(x,y))) {
                    continue;
                }
                List<Vector2> matches = FindMatches(x, y);
                foreach (Vector2 tile in matches) {
                    if (!allMatches.Contains(tile)) {
                        allMatches.Add(tile);
                    }
                }
            }
        }
        
        if (countScore) {
            score += allMatches.Count;
            Debug.Log("SCORE: "+score);
        }
        
        if (allMatches.Count > 0) {
            StartCoroutine(CleanUpBoard(allMatches, countScore));
        }
        // TODO: ensure at least 1 possible match exists
    }

    IEnumerator CleanUpBoard(List<Vector2> allMatches, bool countScore) {
        RemoveTilesAndTriggerMatches(allMatches);
        if (countScore)
            yield return new WaitForSeconds(0.33f);
        
        // empty tiles move down
        ApplyGravity();
        if (countScore)
            yield return new WaitForSeconds(0.33f);
        
        // add new cells to refill board
        AddNewCells();
        
        // re-check in case new tiles have formed new matches
        CheckBoard(countScore);
        yield return null;
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
                if (board[x,y] == null) {
                    for (int above = y+1; above < boardHeight; above++) {
                        if (board[x,above] != null) {
                            float animationTime = 0.33f; // in seconds
                            board[x,above].MoveTile(x, y, animationTime, null);
                            board[x,y] = board[x,above];
                            board[x,above] = null;
                            break;
                        }
                    }
                }
            }
        }
    }

    void RemoveTilesAndTriggerMatches(List<Vector2> tiles) {
        float animationTime = 0.33f;
        foreach (Vector2 tile in tiles) {
            int x = (int) tile.x;
            int y = (int) tile.y;
            
            Tile tileObject = board[x, y];
            TileScriptableObject type = tileObject.GetTileType();
            InventoryManager.GetInstance().EnableItem(type);
            LeanTween.scale(tileObject.gameObject, new Vector3(0, 0, 0), animationTime).setEaseInOutExpo().setDestroyOnComplete(true);
            board[x, y] = null;
        }
    }

    bool IsMatch(int x, int y, string technicalName, List<Vector2> list) {
        TileScriptableObject neighbourType = board[x,y].GetTileType();
        //Debug.Log("Neighbour type: "+neighbourType.technicalName +" to "+technicalName);
        if (neighbourType.technicalName.Equals(technicalName)) {
            list.Add(new Vector2(x,y));
            return true;
        }
        return false;
    }

    List<Vector2> FindMatches(int x, int y) {
        //Debug.Log("Starting for x: "+ x + ", y: "+ y + "(tileType: "+board[x, y].GetTileType()+")");
        List<Vector2> potentialHorizontalMatches = new List<Vector2>();
        List<Vector2> potentialVerticalMatches = new List<Vector2>();
        TileScriptableObject type = board[x,y].GetTileType();

        // look horizontally to the left
        for (int i = x-1; i >= 0; i--) {
            if (!IsMatch(i, y, type.technicalName, potentialHorizontalMatches)) {
                break;
            }
        }

        // look horizontally to the right
        for (int i = x+1; i < boardWidth; i++) {
            if (!IsMatch(i, y, type.technicalName, potentialHorizontalMatches)) {
                break;
            }
        }

        // look vertically below
        for (int i = y-1; i >= 0; i--) {
            if (!IsMatch(x, i, type.technicalName, potentialVerticalMatches)) {
                break;
            }
        }

        // look vertically above
        for (int i = y+1; i < boardHeight; i++) {
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

    void MakeMove(Tile firstTile, Tile secondTile) {
        float animationTime = 0.33f;
        int x1 = firstTile.xIndex;
        int y1 = firstTile.yIndex;
        int x2 = secondTile.xIndex;
        int y2 = secondTile.yIndex;

        if (Mathf.Abs(x1-x2) > 1 || Mathf.Abs(y1-y2) > 1) {
            Debug.LogWarning("Invalid move was attempted!");
            return;
        }

        board[x1, y1] = secondTile;
        board[x2, y2] = firstTile;

        // animation
        firstTile.MoveTile(x2, y2, animationTime, null);
        secondTile.MoveTile(x1, y1, animationTime, CheckBoard);
    }

    void CheckBoard() {
        CheckBoard(true);
    }

}