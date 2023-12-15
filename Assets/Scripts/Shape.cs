using UnityEngine;
using UnityEngine.Tilemaps;

public enum Shape{I, J, L, O, S, T, Z, C}

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

[System.Serializable]
public struct MovementData{
    public Vector2Int down;
    public Vector2Int left;
    public Vector2Int right;

    public string position;

    public void Initialize(string position){
        this.position = position;
        down = Data.DownDirection[position];
        left = Data.LeftDirection[position];
        right = Data.RightDirection[position];
    }

}
