using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Core : MonoBehaviour{
    public Board board;

    void Awake(){
        board = GameObject.Find("Board").GetComponent<Board>();
    }
    
    void Start(){
        board.Set(Shape.O, new Vector3Int(-1,-1,0));
        SetColors();
    }

    public void SetColors(){
        int size = 10;
        for (int x = -size; x < size; x++){
            for (int y = -size; y < size; y++){
                int ty = y;
                int tx = x;
                if(x>-1)tx++;
                if(y>-1)ty++;
                int largest = Mathf.Abs(tx) > Mathf.Abs(ty) ? Mathf.Abs(tx) : Mathf.Abs(ty);
                largest -= 1;
                if (largest < 0) largest = 0;
                if (largest > 7) largest = 7;
                
                Vector3Int pos = new Vector3Int(x,y,0);
                if(board.tilemap.HasTile(pos)){
                    board.tilemap.SetTile(pos, board.reactorTiles[largest]);
                }
            }
        }
    }

    void PrintTileLocations() {
    for (int x = board.tilemap.cellBounds.xMin; x < board.tilemap.cellBounds.xMax; x++) {
        for (int y = board.tilemap.cellBounds.yMin; y < board.tilemap.cellBounds.yMax; y++) {
            Vector3Int position = new Vector3Int(x, y, 0);
            TileBase tile = board.tilemap.GetTile(position);
            if (tile != null) {
                Debug.Log("Tile at position " + position.ToString());
            }
        }
    }
}

    public void CheckRingLayers(){
        for (int i = 1; i < 9; i++){
            CheckRingLayer(i);
        }
        //check for layer 1 ring: if so, check for layer 2 ring, etc, 
    }


    public bool CheckRingLayer(int ringNumber){
        if (ringNumber < 1 || ringNumber > 8){
            print("ring number out of bounds");
            return false;
        }

        int r = ringNumber - 1;

        Vector2Int a = new Vector2Int(-2 - r,  1 + r);
        Vector2Int b = new Vector2Int( 1 + r,  1 + r);
        Vector2Int c = new Vector2Int(-2 - r, -2 - r);
        Vector2Int d = new Vector2Int( 1 + r, -2 - r);

        for (int x = a.x; x <= b.x; x++) {
            for (int y = c.y; y <= a.y; y++) {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (!board.tilemap.HasTile(pos)) {
                    return false;
                }
            }
        }
        return true;
    }

    public void ClearRingLayer(int ringNumber) {
        if (ringNumber < 1 || ringNumber > 8) {
            Debug.LogError("Ring number out of bounds");
            return;
        }

        int r = ringNumber - 1;
        Vector2Int a = new Vector2Int(-2 - r, 1 + r);
        Vector2Int b = new Vector2Int(1 + r, 1 + r);
        Vector2Int c = new Vector2Int(-2 - r, -2 - r);
        Vector2Int d = new Vector2Int(1 + r, -2 - r);

        for (int x = a.x; x <= b.x; x++) {
            Vector3Int pos = new Vector3Int(x, a.y, 0);
            board.tilemap.SetTile(pos, null);
        }

        for (int x = c.x; x <= d.x; x++) {
            Vector3Int pos = new Vector3Int(x, c.y, 0);
            board.tilemap.SetTile(pos, null);
        }

        for(int y = a.y; y >= c.y; y--) {
            Vector3Int pos = new Vector3Int(a.x, y, 0);
            board.tilemap.SetTile(pos, null);
        }

        for(int y = b.y; y >= d.y; y--) {
            Vector3Int pos = new Vector3Int(b.x, y, 0);
            board.tilemap.SetTile(pos, null);
        }

    }

    void shiftBlocks(int layer){
        //algo:
        //start from lowest ring
        //check if ring is full
        //if ring is full, clear it
        //shift all rings above down
        //repeat until no rings are full

        //if ring found on layer 5:
        //  clear layer 5
        // shift layer 6 down, layer 7 down , layer 8 down, etc

        // Only advance to the next row if the current is not cleared
        //         // because the tiles above will fall down when a row is cleared
        //         if (IsLineFull(row)) {
        //             LineClear(row);
        //         } else {
        //             row++;
        //         }
    }
}
