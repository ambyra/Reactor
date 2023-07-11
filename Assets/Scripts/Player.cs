using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode downKey;
    public KeyCode rotateLeftKey;
    public KeyCode rotateRightKey;
    public KeyCode hardDropKey;

    public Piece activePiece;
    public Vector3Int SpawnPosition;
    public Vector2Int FallDirection;
    public Tile PlayerTile; //each player gets own color

    void Start(){
        activePiece = GetComponent<Piece>();
        //activePiece.Initialize(Shape.T, SpawnPosition, FallDirection);
    }

    public void NewPiece(Shape shape){
        activePiece.Initialize(shape, SpawnPosition, FallDirection);
    }

    void NewPiece(){
        //todo: convert to bag system
        int shapes = System.Enum.GetNames(typeof(Shape)).Length;
        Shape shape = (Shape) Random.Range(0, shapes);
        activePiece.Initialize(shape, SpawnPosition, FallDirection);
    }
}




