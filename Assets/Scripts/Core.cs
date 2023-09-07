using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//todo: check ring layers to layer 20
//todo: 2x core?

//todo: steps:
// - lock players

// - get outer ring
// - collapse outer ring
// -repeat till no outer ring

// - rotate board
// - unlock players


public class Core : MonoBehaviour{
    public Board board;

    void Awake(){
        board = GameObject.Find("Board").GetComponent<Board>();
    }
    
    public void Start(){
        board.Clear();
        //O functions as core
        board.Set(Shape.O, board.reactorTiles[0], new Vector3Int(-1,-1,0));
    }

    public Tile  GetTile(Vector3Int position){
        int x = position.x;
        int y = position.y;
        if(x>-1)x++;
        if(y>-1)y++;
        int largest = Mathf.Abs(x) > Mathf.Abs(y) ? Mathf.Abs(x) : Mathf.Abs(y);
        largest -= 1;
        if (largest < 0) largest = 0;
        if (largest > 7) largest = 7;

        return(board.reactorTiles[largest]);
    }

    void SetTiles(){
        for(int x = -8; x < 9; x++){
            for(int y = -8; y < 9; y++){
                Vector3Int pos = new Vector3Int(x,y,0);
                if(board.tilemap.HasTile(pos)) board.tilemap.SetTile(pos, GetTile(pos));
            }
        }
    }

    public int GetSmallestRing(){
        for(int i = 1; i < 9; i++){
            if(CheckRingLayer(i)) return i;
            }
        return 0;
    }
    
    public bool CheckRingLayer(int ringNumber) {
        if (ringNumber < 1 || ringNumber > 8) {
            Debug.LogError("Ring number out of bounds");
            return false;
        }

        int r = ringNumber - 1;
        Vector2Int a = new Vector2Int(-2 - r, 1 + r);
        Vector2Int b = new Vector2Int(1 + r, 1 + r);
        Vector2Int c = new Vector2Int(-2 - r, -2 - r);
        Vector2Int d = new Vector2Int(1 + r, -2 - r);

        for (int x = a.x; x <= b.x; x++) {
            Vector3Int pos = new Vector3Int(x, a.y, 0);
            if(!board.tilemap.HasTile(pos)) return false;
        }

        for (int x = c.x; x <= d.x; x++) {
            Vector3Int pos = new Vector3Int(x, c.y, 0);
            if(!board.tilemap.HasTile(pos)) return false;
        }

        for(int y = a.y; y >= c.y; y--) {
            Vector3Int pos = new Vector3Int(a.x, y, 0);
            if(!board.tilemap.HasTile(pos)) return false;
        }

        for(int y = b.y; y >= d.y; y--) {
            Vector3Int pos = new Vector3Int(b.x, y, 0);
            if(!board.tilemap.HasTile(pos)) return false;
        }
        return true;
    }

    public void ShiftRingLayer(int ringNumber){
        //can optimize if necessary (a->b == c->d)
        if (ringNumber < 1 || ringNumber > 8) {
            Debug.LogError("Ring number out of bounds");
            return;
        }

        int r = ringNumber;
        Vector2Int a = new Vector2Int(-2 - r, 1 + r);
        Vector2Int b = new Vector2Int(1 + r, 1 + r);
        Vector2Int c = new Vector2Int(-2 - r, -2 - r);
        Vector2Int d = new Vector2Int(1 + r, -2 - r);

        for (int x = a.x; x <= b.x; x++) {
            Vector3Int pos = new Vector3Int(x, a.y, 0);
            Vector3Int newPos = new Vector3Int(x, a.y - 1, 0);
            Tile tile = board.tilemap.GetTile<Tile>(pos);
            board.tilemap.SetTile(pos, null);
            board.tilemap.SetTile(newPos, tile);
        }

        for (int x = c.x; x <= d.x; x++) {
            Vector3Int pos = new Vector3Int(x, c.y, 0);
            Vector3Int newPos = new Vector3Int(x, c.y + 1, 0);
            Tile tile = board.tilemap.GetTile<Tile>(pos);
            board.tilemap.SetTile(pos, null);
            board.tilemap.SetTile(newPos, tile);
        }

        for(int y = a.y; y >= c.y; y--) {
            Vector3Int pos = new Vector3Int(a.x, y, 0);
            Vector3Int newPos = new Vector3Int(a.x + 1, y, 0);
            Tile tile = board.tilemap.GetTile<Tile>(pos);
            board.tilemap.SetTile(pos, null);
            board.tilemap.SetTile(newPos, tile);
        }

        for(int y = b.y; y >= d.y; y--) {
            Vector3Int pos = new Vector3Int(b.x, y, 0);
            Vector3Int newPos = new Vector3Int(b.x - 1, y, 0);
            Tile tile = board.tilemap.GetTile<Tile>(pos);
            board.tilemap.SetTile(pos, null);
            board.tilemap.SetTile(newPos, tile);
        }
    }

    public bool CheckBoard(){
        int smallestRing = GetSmallestRing();
        if (smallestRing == 0) return false;
        for(int i = smallestRing; i < 9; i++){
            ShiftRingLayer(i);
        }

        SetTiles();
        return true;
    }
}