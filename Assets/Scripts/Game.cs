using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour{
    public GameObject PlayerPrefab;
    public List<Player> Players = new List<Player>();
    public Board board;
    public Tilemap tilemap;
    public Settings settings;
    public Core core;
    public Player player1;

    //todo:
    // - foreach player drop piece:
    //      - each player gets own piece color
    // - when no pieces left, rotate board
    //match colors to reactor layers

    //todo: wall kicks for l/r players
    //todo: timer for all objects

    void Awake(){
        board = GameObject.Find("Board").GetComponent<Board>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        settings = GameObject.Find("Game").GetComponent<Settings>();
        core = GameObject.Find("Core").GetComponent<Core>();

        player1 = AddPlayer("top");
        player1.NewPiece();
        //addPlayer("bottom");
        //addPlayer("left");
        //addPlayer("right");
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.N))board.RotateWithAnimation(player1.NewPiece);
    }
    
    Player AddPlayer(string position){
        print("adding player " + position);
        GameObject playerObject = Instantiate(PlayerPrefab, new Vector3(0,0,0), Quaternion.identity, transform);
        playerObject.name = position;
        Player player = playerObject.GetComponent<Player>();
        player.SpawnPosition = Data.SpawnPositions[position];
        player.FallDirection = Data.FallDirections[position];
        Players.Add(player);
        return player;
    }

}
