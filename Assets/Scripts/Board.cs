using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour{
    public Tilemap tilemap;
    public Game game;
    public ShapeData[] shapes;
    public Vector2Int boardSize = new Vector2Int(30, 30);

    public RectInt Bounds {
        get{
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake(){
        game = GameObject.Find("Game").GetComponent<Game>();
        tilemap = game.Tilemap;
        initShapes();
    }

    void initShapes(){
        for (int i = 0; i < shapes.Length; i++) {shapes[i].Initialize();}
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

    public void Rotate(int direction){}

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
