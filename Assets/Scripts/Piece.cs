using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class Piece : MonoBehaviour{
    public static UnityEvent LockEvent;

    public ShapeData data;
    public Vector3Int[] cells;
    public Tile tile;

    public Vector3Int position;
    public Vector2Int fallDirection;

    public int beats;
    public int rotationIndex;

    public float lockTime;
    public float lockDelay = 2.0f;
    

    public bool isLocked;
    public bool isBeatEvent;
    public bool isMoveable;

    public Game game;
    public Board board;
    public Core core;
    public Settings settings;
    public BeatController beat;

    private int rotateDirection = 0;
    private Vector2Int moveDirection = Vector2Int.zero;

    void Awake(){
        game = GameObject.Find("Game").GetComponent<Game>();
        LockEvent = new UnityEvent();
        
        isLocked = true;
        isBeatEvent = false;
        isMoveable = false;
    }

    void Start(){
        settings = game.settings;
        board = game.board;
        core = game.core;
        beat = game.beat;
        beat.BeatEvent.AddListener(onBeatEvent);
        lockDelay = beat.beatLength * 4;
    }

    public void Initialize(Shape shape, Tile tile, Vector3Int position, Vector2Int fallDirection){
        if (!isLocked) return; //only new shape if locked
        isLocked = false;

        this.tile = tile;
        this.position = position;
        this.fallDirection = fallDirection;

        beats = 0;
        rotationIndex = 0;

        data = board.shapes[(int)shape];    
        cells = new Vector3Int[data.cells.Length];
        for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    public void onBeatEvent(){if (!isLocked) isBeatEvent = true;}

    public void Update(){
        if (isLocked) return;
        lockTime += Time.deltaTime;

        board.Clear(this);

        if (isBeatEvent){
            isBeatEvent = false;
            beats++;
            if (beats > 15*4){
                Kill();
                return;
            }
            
            if(moveDirection != Vector2Int.zero){
                translate(moveDirection);
                moveDirection = Vector2Int.zero;
            }

            if(beats%4 == 0) Step();
            if(isLocked) return;
        }

        if(isLocked) return;

        if (rotateDirection != 0){
            rotate(rotateDirection);
            rotateDirection = 0;
        }
        board.Set(this);
    }

    public void Drop(){
        translate(fallDirection);
        isMoveable = false;
    }

    void Step(){
        bool isValidStep = translate(fallDirection);
        if(isValidStep) lockTime = 0;
        if(lockTime > lockDelay) Lock();
    }

    public void Rotate(int direction){
        rotateDirection = direction;
    }

    public void Move(Vector2Int direction){
        moveDirection = direction;
    }

    void Lock(){
        isLocked = true;
        board.Clear(this);
        board.SetLocked(this);
        LockEvent.Invoke();
    }

    public void Kill(){
        isLocked = true;
        board.Clear(this);
        LockEvent.Invoke();
    }

    void translateTiles(Vector2Int translation){
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        position = newPosition;
    }

    bool translate(Vector2Int translation){
        if(translation == Vector2Int.zero) return true;

        Vector3Int originalPosition = position;
        translateTiles(translation);
        bool isValid = board.IsValidPosition(this, position);
        if(!isValid){
            position = originalPosition;
            return false;
        }
        return true;
    }

    void rotateTiles(int direction){
        float[] matrix = Data.RotationMatrix;
        for (int i = 0; i < cells.Length; i++){
            Vector3 cell = cells[i];
            int x, y;
            switch (data.shape){
                case Shape.I:
                case Shape.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }
            cells[i] = new Vector3Int(x, y, 0);
        }
    }

    void rotate(int direction){
        int originalRotation = rotationIndex;
        rotationIndex = wrap(rotationIndex + direction, 0, 4);
        rotateTiles(direction);
        if (!board.IsValidPosition(this, position)){
            if(!tryWallKick(direction)){
                rotateTiles(-direction);
                rotationIndex = originalRotation;
            }
        }

    }

    bool tryMove(Vector2Int translation){
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        return board.IsValidPosition(this, newPosition);
    }

    bool tryWallKick(int rotationDirection){
        int wallKickIndex = getWallKickIndex(rotationIndex, rotationDirection);
        for (int i = 0; i < data.wallKicks.GetLength(1); i++){
            Vector2Int translation = data.wallKicks[wallKickIndex, i];
            if(tryMove(translation)){
                translate(translation);
                return true;
            }
        }
        return false;
    }

    int getWallKickIndex(int rotationIndex, int rotationDirection){
        int wallKickIndex = rotationIndex * 2;
        if (rotationDirection < 0) wallKickIndex--;
        return wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }

    int wrap(int input, int min, int max){
        if (input < min) return max - (min - input) % (max - min);
        return min + (input - min) % (max - min);
    }
}
