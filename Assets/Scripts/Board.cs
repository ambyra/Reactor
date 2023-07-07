using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour{
    public Tilemap tilemap;
    public Game game;
    //git

    public ShapeData[] shapes;
    public Vector2Int boardSize = new Vector2Int(30, 30);

    public RectInt Bounds {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        tilemap = game.Tilemap;
        //activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < shapes.Length; i++) {
            shapes[i].Initialize();
        }
    }

    // public Piece SpawnPiece(Shape shape, Vector3Int position){
    //     ShapeData shapeData = shapes[(int)shape];

    //     activePiece.Initialize(position, shapeData);

    //     // if (IsValidPosition(activePiece, position)) {
    //     //     Set(activePiece);
    //     // } else {
    //     //     //GameOver();
    //     // }
    //     return activePiece;
    // }

    // public Piece SpawnPiece()
    // {
    //     int random = Random.Range(0, shapes.Length);
    //     ShapeData shapeData = shapes[random];
    //     activePiece.Initialize(game.CurrentPlayer.SpawnPosition, shapeData);
    //     return activePiece;
    // }
    
    public void GameOver()
    {
        tilemap.ClearAllTiles();
        Debug.Log("Game Over!");
    }

    public void Set(Piece piece){
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece){
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        return true;

        // RectInt bounds = Bounds;

        // // The position is only valid if every cell is valid
        // for (int i = 0; i < piece.cells.Length; i++)
        // {
        //     Vector3Int tilePosition = piece.cells[i] + position;

        //     // An out of bounds tile is invalid
        //     if (!bounds.Contains((Vector2Int)tilePosition)) {
        //         return false;
        //     }

        //     // A tile already occupies the position, thus invalid
        //     if (tilemap.HasTile(tilePosition)) {
        //         return false;
        //     }
        // }

        // return true;
    }

    public void ClearLines()
    {
        Debug.Log("Clearing lines");
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row)) {
                LineClear(row);
            } else {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position)) {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        Debug.Log("Clearing line " + row);
        RectInt bounds = Bounds;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }

}
