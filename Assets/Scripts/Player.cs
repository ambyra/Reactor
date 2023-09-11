using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour{
    public Piece activePiece;

    public Vector3Int SpawnPosition;
    public Vector2Int FallDirection;
    public Tile PlayerTile; //todo: each player gets own color

    public List<int> pieces = new();

    void Awake(){
        activePiece = GetComponent<Piece>();
    }

    public void Initialize(Tile playerTile, Vector3Int spawnPosition, Vector2Int fallDirection){
        PlayerTile = playerTile;
        SpawnPosition = spawnPosition;
        FallDirection = fallDirection;
    }

    public void NewPiece(Shape shape){
        activePiece.Initialize(shape, PlayerTile, SpawnPosition, FallDirection);
    }

    public void NewPiece(){

        if (pieces.Count == 0) pieces = new List<int>(){0,1,2,3,4,5,6};

        int index = Random.Range(0, pieces.Count);
        int shapeIndex = pieces[index];
        pieces.RemoveAt(index);

        Shape shape = (Shape) shapeIndex;
        activePiece.Initialize(shape, PlayerTile, SpawnPosition, FallDirection);
    }
}




