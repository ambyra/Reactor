using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour{
    public Tilemap tilemap;
    public Game game;
    public ShapeData[] shapes;
    public Vector2Int boardSize = new Vector2Int(30, 30);

    public RectInt Bounds{
        get{
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake(){
        game = GameObject.Find("Game").GetComponent<Game>();
        tilemap = game.Tilemap;
        for(int i = 0; i < shapes.Length; i++){
            shapes[i].Initialize();
        }
    }

    public void GameOver()
    {
        tilemap.ClearAllTiles();
        Debug.Log("Game Over!");
    }

    public void Set(Piece piece){
        for (int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Set(Shape shape, Vector3Int position){
        ShapeData data = shapes[(int)shape];
        for (int i = 0; i < data.cells.Length; i++){
            Vector3Int tilePosition = (Vector3Int)data.cells[i] + position;
            tilemap.SetTile(tilePosition, data.tile);
        }
    }

    public void Rotate(int direction){
        int d = direction;
        //get cells
        //make new array
        //rotate cells
        //set cells in new array

        float[] m = Data.RotationMatrix;
        List<Vector3Int> newPos = new List<Vector3Int>();

        for (int x = -5; x<5; x++){ for (int y = -5; y<5; y++){
            if (tilemap.HasTile(new Vector3Int(x, y, 0))){
                print("tile at " + x + ", " + y);
                // int newX = Mathf.RoundToInt((x * m[0] * d) + (y * m[1] * d));
                // int newY = Mathf.RoundToInt((x * m[2] * d) + (y * m[3] * d));
                int newX = y;
                int newY = -x;
                newPos.Add(new Vector3Int(newX, newY-1, 0));
            }
        }}
        tilemap.ClearAllTiles();
        for (int i = 0; i < newPos.Count; i++){
            tilemap.SetTile(newPos[i], shapes[(int)Shape.O].tile);
        }
    }

    



    //x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
    //y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));

    // for (int x = 0; x < bounds.size.x; x++) {
    //     for (int y = 0; y < bounds.size.y; y++) {
    //         TileBase tile = allTiles[x + y * bounds.size.x];
    //         if (tile != null) {
    //             Vector3Int tilePosition = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);
    //             Vector3Int offset = tilePosition - center;
    //             Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90));
    //             Vector3Int newPosition = center + new Vector3Int(Mathf.RoundToInt(offset.x * matrix[0] + offset.y * matrix[1]), Mathf.RoundToInt(offset.x * matrix[2] + offset.y * matrix[3]), 0);
    //             tilemap.SetTile(newPosition, tile);
    //             tilemap.SetTile(tilePosition, null);
    //         }
    //     }
    // }
    // }

    public void Clear(Piece piece){
        for (int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition)) {
                return false;
            }
        }

        return true;
    }
}
