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

    //idea: if piece collides with other piece, 
    //      pieces merge, both players control, 
    //      can clear more than 4 layers at once

    //todo:
    // - add players
    // - grid over stars background

    //sequence: 
    // - count players
    // - foreach player drop piece:
    //      - each player gets own piece color
    //      - if piece collides with other piece, clear both
    //      - if piece collides with core, lock it, turn white
    //      - if piece collides with wall, lock it, turn black
    // - when no pieces left, rotate board

    //todo: wall kicks for sides
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
        //todo: rotate auto after 3 beats
        //      - clear unset tiles
        //      - rotate board

        if (Input.GetKeyDown(KeyCode.N)){
            board.RotateWithAnimation(player1.NewPiece);
            // board.Rotate();
            // player1.NewPiece();

        }

        if (Input.GetKeyDown(KeyCode.K)){player1.activePiece.RotateRight = true;}
        if (Input.GetKeyDown(KeyCode.J)){player1.activePiece.RotateLeft = true;}

        if (Input.GetKeyDown(KeyCode.S)){player1.activePiece.MoveLeft = true;}
        if (Input.GetKeyDown(KeyCode.F)){player1.activePiece.MoveRight = true;}
        if (Input.GetKeyDown(KeyCode.D)){player1.activePiece.MoveDown = true;}
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
