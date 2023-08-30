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

    //todo: red bars going up and down for reactor temperature
    //avoid meltdown

    //todo: create separate tilemap for core (?)
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
        //player1.NewPiece();
        //addPlayer("bottom");
        //addPlayer("left");
        //addPlayer("right");
    }

    void Update(){
        //if (Input.GetKeyDown(KeyCode.N))board.RotateWithAnimation(player1.NewPiece);

        if (Input.GetKeyDown(KeyCode.N)){
            player1.activePiece.isLocked = true;
            board.Rotate();
            player1.NewPiece();
        }

        if(Input.GetKeyDown(KeyCode.R)){
            core.CheckRingLayers();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            bool isRing = core.CheckRingLayer(1);
            if(isRing) core.ClearRingLayer(1);
        }

        clearRingLayers();

        //todo: kill piece as soon as it locks, block input etc. 
        //todo: rotate board when all players pieces are locked
        //nextPiece();
    }

    void clearRingLayers() {
        for (int i = 1; i <= 8; i++) {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i)) {
                bool isRing = core.CheckRingLayer(i);
                if (isRing) {
                    core.ClearRingLayer(i);
                }
            }
        }
    }



    void nextPiece(){
        // if (player1.activePiece.isLocked){
        //     //board.RotateWithAnimation(player1.NewPiece);
        //     board.Rotate();
        //     player1.NewPiece();
        // }
    }
    
    Player AddPlayer(string position){
        //print("adding player " + position);
        GameObject playerObject = Instantiate(PlayerPrefab, new Vector3(0,0,0), Quaternion.identity, transform);
        playerObject.name = position;
        Player player = playerObject.GetComponent<Player>();
        player.SpawnPosition = Data.SpawnPositions[position];
        player.FallDirection = Data.FallDirections[position];
        Players.Add(player);
        return player;
    }

}
