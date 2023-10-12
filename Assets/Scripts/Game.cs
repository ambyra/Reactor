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
    public Tilemap fxTilemap;
    public Settings settings;
    public Core core;
    public BeatController beat;

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

    //todo: merge cymk colors when atoms cross?
    // - or, just delete both pieces

    //todo: kill piece after 15? steps

    void Awake(){
        board = GameObject.Find("Board").GetComponent<Board>();
        //tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>(); //set in editor
        settings = GameObject.Find("Game").GetComponent<Settings>();
        core = GameObject.Find("Core").GetComponent<Core>();
        beat = GameObject.Find("Beat").GetComponent<BeatController>();


        player1 = AddPlayer("top");
        player1.Initialize(board.playerTiles[1], Data.SpawnPositions["top"], Data.FallDirections["top"]);
    }


    void Update(){
        if (Input.GetKeyDown(KeyCode.N)) player1.NewPiece();
        if (Input.GetKeyDown(KeyCode.O)) player1.NewPiece(Shape.O);
        if (Input.GetKeyDown(KeyCode.I)) player1.NewPiece(Shape.I);
        if (Input.GetKeyDown(KeyCode.T)) player1.NewPiece(Shape.T);

        if (Input.GetKeyDown(KeyCode.C)) core.CheckBoard();
        if (Input.GetKeyDown(KeyCode.X)){
            board.Clear();
            core.Start();
        } 

        if (Input.GetKeyDown(KeyCode.R) && player1.activePiece.isLocked){
            board.RotateWithAnimation();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            core.MoveRingToFxLayer(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)){
            core.MoveRingToFxLayer(3);
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

    public void OnBoardRotateComplete(){
        print ("board rotate complete");
        }

    public void OnPieceLock(){
        print ("piece lock");
        }
        
}
