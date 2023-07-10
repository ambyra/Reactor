using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour{
    public GameObject PlayerPrefab;
    public List<Player> Players = new List<Player>();
    public Board Board;
    public Tilemap Tilemap;
    public Settings Settings;
    public Core Core;

    void Awake(){
        Board = GameObject.Find("Board").GetComponent<Board>();
        Tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        Settings = GameObject.Find("Game").GetComponent<Settings>();
        Core = GameObject.Find("Core").GetComponent<Core>();

        //addPlayer("top");
        //addPlayer("bottom");
        //addPlayer("left");
        //addPlayer("right");
    }

    void Update(){
        //todo: rotate auto after 3 beats
        //      - clear unset tiles
        //      - rotate board
        
        if (Input.GetKeyDown(KeyCode.R)) Board.Rotate(1);
    }
    
    void addPlayer(string position){
        GameObject playerObject = Instantiate(PlayerPrefab, new Vector3(0,0,0), Quaternion.identity, transform);
        playerObject.name = position;
        Player player = playerObject.GetComponent<Player>();
        player.SpawnPosition = Data.SpawnPositions[position];
        player.FallDirection = Data.FallDirections[position];
        Players.Add(player);
    }

}
