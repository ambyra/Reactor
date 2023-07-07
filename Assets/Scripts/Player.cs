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
        activePiece.Initialize(SpawnPosition, Shape.T);
    }
}




