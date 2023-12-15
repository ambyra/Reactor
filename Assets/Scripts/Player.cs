using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour{
    public Piece activePiece;

    
    public Tile playerTile; //player color

    public enum PlayerPosition{Top, Bottom, Left, Right};
    public PlayerPosition position;
    public Vector3Int spawnPosition;
    public MovementData movementData;
    public List<int> pieces = new();

    public bool isActive;
    public bool isPieceActive(){
        if (activePiece.isLocked) return false;
        return true;
    }

    void Awake(){
        activePiece = GetComponent<Piece>();
        isActive = false;

        spawnPosition = Data.SpawnPositions[position.ToString().ToLower()];
        movementData = new MovementData();
        movementData.Initialize(position.ToString().ToLower());
    }

    void moveLeft(){activePiece.Move(movementData.left);}

    void moveRight(){activePiece.Move(movementData.right);}

    void drop(){activePiece.Drop();}


    public void NewPiece(Shape shape){
        activePiece.Initialize(shape, playerTile, spawnPosition, movementData.down);
    }

    public void NewPiece(){

        if (pieces.Count == 0) pieces = new List<int>(){0,1,2,3,4,5,6};

        int index = Random.Range(0, pieces.Count);
        int shapeIndex = pieces[index];
        pieces.RemoveAt(index);

        Shape shape = (Shape) shapeIndex;
        activePiece.Initialize(shape, playerTile, spawnPosition, movementData.down);
    }

    public void ClearPiece(){
        activePiece.Kill();
    }
}




