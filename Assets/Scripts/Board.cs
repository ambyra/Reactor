using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour{
    public Tilemap tilemap;
    public Game game;
    public ShapeData[] shapes;
    public Vector2Int boardSize = new Vector2Int(100, 100);

    public List<Tile> reactorTiles = new List<Tile>();
    public List<Tile> playerTiles = new List<Tile>();

    private void Awake(){
        game = GameObject.Find("Game").GetComponent<Game>();
        tilemap = game.tilemap;
        for(int i = 0; i < shapes.Length; i++){
            shapes[i].Initialize();
        }
    }

    public RectInt Bounds{
        get{
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
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

    public void RotateWithAnimation(Action onComplete = null){
        StartCoroutine(rotateTilemap(90f, 0.5f, onComplete));
    }

    private IEnumerator rotateTilemap(float angle = 90f, float duration = 0.5f , Action onComplete = null){
        float elapsed = 0f; // Elapsed time since rotation started
        Quaternion startRotation = tilemap.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, -90f);

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            tilemap.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        tilemap.transform.rotation = endRotation;
        Rotate();

        if (onComplete != null) onComplete(); 
    }

    public class TileData{
        public Vector3Int position;
        public TileBase tile;
    }

    public void Rotate(){Rotate(1);}

    public void Rotate(int direction){
        tilemap.transform.rotation = Quaternion.Euler(0, 0, 0);

        List<TileData> tiles = new List<TileData>();

        int d = direction;
        int size = 10;
        int s = size;
        //List<Vector3Int> newPos = new List<Vector3Int>();
        //List<TileBase> tiles = new List<TileBase>();


        for (int x = -s; x<s; x++){
            for (int y = -s; y<s; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(tilePosition)){
                    TileBase tile = tilemap.GetTile(tilePosition);
                    Vector3Int newPosition = new Vector3Int(y, -x - 1, 0);
                    tiles.Add(new TileData{position = newPosition, tile = tile});
                    //Tile tileData = tile as Tile;
                    // tiles.Add(tile);
                    // newPos.Add(new Vector3Int(y, -x - 1, 0));
            }
        }}

        tilemap.ClearAllTiles();

        // for (int i = 0; i < newPos.Count; i++){
        //     //todo: replace w original tyle type
        //     tilemap.SetTile(newPos[i], shapes[(int)Shape.O].tile);
        // }

        for (int i = 0; i < tiles.Count; i++){
            tilemap.SetTile(tiles[i].position, tiles[i].tile);
        }
    }

    public void Clear(Piece piece){
        for (int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position){
        RectInt bounds = Bounds;
        for (int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + position;
            //if (!bounds.Contains((Vector2Int)tilePosition)) return false;
            if (tilemap.HasTile(tilePosition))return false;
            
        }
        return true;
    }
}
