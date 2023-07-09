using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start(){
        activePiece = GetComponent<Piece>();
        activePiece.Initialize(Shape.T, SpawnPosition, FallDirection);
    }

    void SpawnPiece(Shape shape){
        activePiece.Initialize(shape, SpawnPosition, FallDirection);
    }

    void SpawnRandomPiece(){
        //todo: convert to bag system
        int shapes = System.Enum.GetNames(typeof(Shape)).Length;
        Shape shape = (Shape) Random.Range(0, shapes);
        activePiece.Initialize(shape, SpawnPosition, FallDirection);
    }

    bool isValidPosition(){
                // if (IsValidPosition(activePiece, position)) {
        //     Set(activePiece);
        // } else {
        //     //GameOver();
        // }
        return false;
    }



}




