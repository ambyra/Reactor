using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour{
    //todo: - function to check for core rings

    public Board board;

    void Awake(){
        board = GameObject.Find("Board").GetComponent<Board>();
    }
    
    void Start(){
        board.Set(Shape.C, new Vector3Int(-1,-1,0));
        //board.Set(Shape.O, new Vector3Int(2,-1,0));
        //board.tilemap.SetTile(newPos[i], board.shapes[(int)Shape.O].tile);
    }

    void checkForRings(){
        //check for layer 1 ring: if so, check for layer 2 ring, etc, 
    }

    // public void ClearLines(){
    //     print("Clearing lines");
    //     RectInt bounds = Bounds;
    //     int row = bounds.yMin;

    //     // Clear from bottom to top
    //     while (row < bounds.yMax)
    //     {
    //         // Only advance to the next row if the current is not cleared
    //         // because the tiles above will fall down when a row is cleared
    //         if (IsLineFull(row)) {
    //             LineClear(row);
    //         } else {
    //             row++;
    //         }
    //     }
    // }

    // public bool IsLineFull(int row)
    // {
    //     RectInt bounds = Bounds;
    //     for (int col = bounds.xMin; col < bounds.xMax; col++){
    //         Vector3Int position = new Vector3Int(col, row, 0);
    //         // The line is not full if a tile is missing
    //         if (!tilemap.HasTile(position)) {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    // public void LineClear(int row){
    //     print ("Clearing line " + row);
    //     RectInt bounds = Bounds;

    //     // Clear all tiles in the row
    //     for (int col = bounds.xMin; col < bounds.xMax; col++){
    //         Vector3Int position = new Vector3Int(col, row, 0);
    //         tilemap.SetTile(position, null);
    //     }

    //     // Shift every row above down one
    //     while (row < bounds.yMax){
    //         for (int col = bounds.xMin; col < bounds.xMax; col++){
    //             Vector3Int position = new Vector3Int(col, row + 1, 0);
    //             TileBase above = tilemap.GetTile(position);

    //             position = new Vector3Int(col, row, 0);
    //             tilemap.SetTile(position, above);
    //         }
    //         row++;
    //     }
    // }
}
