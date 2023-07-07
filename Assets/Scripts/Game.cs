using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour{
    public GameObject PlayerPrefab;
    public List<Player> Players = new List<Player>();
    public Board Board;
    public Tilemap Tilemap;
    
    public Player CurrentPlayer;

    void Awake(){
        Board = GameObject.Find("Board").GetComponent<Board>();
        Tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        addPlayer("top");
        addPlayer("bottom");
        // addPlayer("left");
        // addPlayer("right");

        CurrentPlayer = Players[0];
    }

    // void Start(){
    //     initCore();
    // }

    void addPlayer(string position){
        GameObject playerObject = Instantiate(PlayerPrefab, new Vector3(0,0,0), Quaternion.identity, transform);
        playerObject.name = position;
        Player player = playerObject.GetComponent<Player>();
        player.SpawnPosition = Data.SpawnPositions[position];
        player.FallDirection = Data.FallDirections[position];
        Players.Add(player);
    }

    // public void nextPlayer(){
    //     int index = Players.IndexOf(CurrentPlayer);
    //     index = (index + 1) % Players.Count;
    //     CurrentPlayer = Players[index];
    // }

    void initCore(){
        //Piece core = Board.SpawnPiece(Shape.O, new Vector3Int(-1,-1,0));
        //Board.SpawnPiece();
    }

}
