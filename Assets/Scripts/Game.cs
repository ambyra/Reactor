using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    void Awake(){
        
        board = GameObject.Find("Board").GetComponent<Board>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        settings = GameObject.Find("Game").GetComponent<Settings>();
        core = GameObject.Find("Core").GetComponent<Core>();
        beat = GameObject.Find("Beat").GetComponent<BeatController>();

        //CreatePlayers();
        tilemap.transform.rotation = Quaternion.Euler(0,0,0);
    }

    void Start(){
        //Piece.LockEvent.AddListener(OnPieceLock);
        // Players[1].isActive = true;
        PlayersNewPiece();
    }

    void PlayersNewPiece(){
        foreach (Player player in Players){
            if(player.isActive) player.NewPiece();
        }
    }

    void Update(){
        // if (Input.GetKeyDown(KeyCode.N)){Players[0].NewPiece();}
        // if (Input.GetKeyDown(KeyCode.M)){Players[1].NewPiece();}
        // if (Input.GetKeyDown(KeyCode.B)){Players[2].NewPiece();}
        // if (Input.GetKeyDown(KeyCode.V)){Players[3].NewPiece();}

        if (Input.GetKeyDown(KeyCode.P)){ ClearPieces(); }

        //if (Input.GetKeyDown(KeyCode.C)) core.CheckBoard();
        if (Input.GetKeyDown(KeyCode.X)){
            board.Clear();
            core.Start();
        }
    }

    void ClearPieces(){
        foreach (Player player in Players){
            player.ClearPiece();
        }
    }

    bool isActivePieceOnBoard(){
        foreach (Player player in Players){
            if (player.isPieceActive()) return true;
        }
        return false;
    }

    // void CreatePlayers(){
    //     foreach (string position in Data.SpawnPositions.Keys){
    //         Player player = CreatePlayer(position);
    //     }
    //     for (int i = 0; i < Players.Count; i++){
    //         Player player = Players[i];
    //         player.Initialize(board.playerTiles[i], player.SpawnPosition, player.FallDirection);
    //     }
    // }
    
    // Player CreatePlayer(string position){
    //     GameObject playerObject = Instantiate(PlayerPrefab, new Vector3(0,0,0), Quaternion.identity, transform);
    //     playerObject.name = position;
    //     Player player = playerObject.GetComponent<Player>();
    //     player.spawnPosition = Data.SpawnPositions[position];
    //     player.movementData = new MovementData();
    //     Players.Add(player);
    //     return player;
    // }

    public void OnBoardRotateComplete(){
        print ("board rotate complete");
        }

    public async void OnPieceLock(){
        if (isActivePieceOnBoard()) return;
        await core.CollapseSmallestRingWithAnimation();
        await core.CollapseSmallestRingWithAnimation();
        await core.CollapseSmallestRingWithAnimation();
        await core.CollapseSmallestRingWithAnimation();
        await board.RotateWithAnimation();
        PlayersNewPiece();
    }
        
}
