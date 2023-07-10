using UnityEngine;
using UnityEngine.Tilemaps;

public enum Shape{I, J, L, O, S, T, Z}

[System.Serializable]
public struct ShapeData{
    public Tile tile;
    public Shape shape;

    public Vector2Int[] cells;
    public Vector2Int[,] wallKicks;

    public void Initialize(){
        cells = Data.Cells[shape];
        wallKicks = Data.WallKicks[shape];
    }
}
