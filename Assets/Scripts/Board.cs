using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using System.Threading.Tasks;

public class Board : MonoBehaviour{
    public UnityEvent RotateCompleteEvent;

    public Tilemap tilemap;
    public Game game;
    public BeatController beat;

    public ShapeData[] shapes;
    public Vector2Int boardSize = new Vector2Int(100, 100);

    public List<Tile> reactorTiles = new List<Tile>();
    public List<Tile> playerTiles = new List<Tile>();

    private void Awake(){
        game = GameObject.Find("Game").GetComponent<Game>();
        tilemap = game.tilemap;
        beat = game.beat;

        for(int i = 0; i < shapes.Length; i++){
            shapes[i].Initialize();
        }
    }

    public class TileData{
        public Vector3Int position;
        public TileBase tile;
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
            tilemap.SetTile(tilePosition, piece.tile);
        }
    }

    public void SetLocked(Piece piece){
        for (int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, game.core.GetTile(tilePosition));
        }
    }

    public void Set(Shape shape, Vector3Int position){
        ShapeData data = shapes[(int)shape];
        for (int i = 0; i < data.cells.Length; i++){
            Vector3Int tilePosition = (Vector3Int)data.cells[i] + position;
            tilemap.SetTile(tilePosition, data.tile);
        }
    }

    public void Set(Shape shape, Tile tile, Vector3Int position){
        ShapeData data = shapes[(int)shape];
        for (int i = 0; i < data.cells.Length; i++){
            Vector3Int tilePosition = (Vector3Int)data.cells[i] + position;
            tilemap.SetTile(tilePosition, tile);
        }
    }

    public async Task RotateWithAnimation(){
        var end = Time.time + beat.beatLength * 4.0f;
        Quaternion startRotation = tilemap.transform.rotation;
        //rotate animation
        while (Time.time < end){
            tilemap.transform.Rotate(0, 0, -90f * Time.deltaTime / (beat.beatLength * 4.0f));
            await Task.Yield();
        }
        //reset rotation
        tilemap.transform.rotation = startRotation;
        //rotate board
        RotateTilemap();
    }


    public void RotateTilemap(int direction = 1){
        tilemap.transform.rotation = Quaternion.Euler(0, 0, 0);

        List<TileData> tiles = new List<TileData>();

        int d = direction;
        int size = 10;
        int s = size;

        for (int x = -s; x<s; x++){
            for (int y = -s; y<s; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(tilePosition)){
                    TileBase tile = tilemap.GetTile(tilePosition);
                    Vector3Int newPosition = new Vector3Int(y, -x - 1, 0);
                    tiles.Add(new TileData{position = newPosition, tile = tile});
            }
        }}

        tilemap.ClearAllTiles();
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

    public void Clear(){tilemap.ClearAllTiles();}

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