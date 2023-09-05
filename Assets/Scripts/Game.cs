using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour{
    public GameObject PlayerPrefab;
    public List<Player> Players = new();
    public Board board;
    public Tilemap tilemap;
    public Settings settings;
    public Core core;
    public Player player1;

    //todo: create separate tilemap for core (?)
    //todo: hard drop
    //todo: animate core waves coming out w music
    //todo: event system for timing and animations

    //todo:
    // - foreach player drop piece:
    //      - each player gets own piece color
    //      - when no pieces left, rotate board

    //todo: wall kicks for l/r players
    //todo: timer for all objects
    //todo: kill piece as soon as it locks, block input etc. 
    //todo: rotate board when all players pieces are locked

    void Awake(){
        board = GameObject.Find("Board").GetComponent<Board>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        settings = GameObject.Find("Game").GetComponent<Settings>();
        core = GameObject.Find("Core").GetComponent<Core>();

        player1 = AddPlayer("top");
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.N)){
            player1.activePiece.isLocked = true;
            player1.NewPiece();
        }

        if (Input.GetKeyDown(KeyCode.O)){
            player1.activePiece.isLocked = true;
            player1.NewPiece(Shape.O);
        }

        if (Input.GetKeyDown(KeyCode.I)){
            player1.activePiece.isLocked = true;
            player1.NewPiece(Shape.I);
        }

        if (Input.GetKeyDown(KeyCode.T)){
            player1.activePiece.isLocked = true;
            player1.NewPiece(Shape.T);
        }

        if(Input.GetKeyDown(KeyCode.R)){
            board.RotateWithAnimation();
        }

        if (Input.GetKeyDown(KeyCode.C)){
            core.CheckBoard();
        }
    }
    
    Player AddPlayer(string position){
        GameObject playerObject = Instantiate(PlayerPrefab, new Vector3(0,0,0), Quaternion.identity, transform);
        playerObject.name = position;
        Player player = playerObject.GetComponent<Player>();
        player.SpawnPosition = Data.SpawnPositions[position];
        player.FallDirection = Data.FallDirections[position];
        Players.Add(player);
        return player;
    }
}
