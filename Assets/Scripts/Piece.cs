using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Piece : MonoBehaviour{
    public UnityEvent LockEvent;

    public ShapeData data;
    public Vector3Int[] cells;
    public Tile tile;

    public Vector3Int position;
    public Vector2Int direction;

    public int rotationIndex;
    public bool isLocked;

    public Game game;
    public Board board;
    public Core core;
    public Settings settings;
    public BeatController beat;

    // float stepTime;
    // public int stepCount;

    // private float moveTime;
    public float lockTime;

    public bool canStep;
    public bool canMove;
    public bool canRotate;

    public int beats;

    void Awake(){
        game = GameObject.Find("Game").GetComponent<Game>();
        settings = game.settings;
        board = game.board;
        core = game.core;
        beat = game.beat;

        LockEvent.AddListener(game.OnPieceLock);

        beat.BeatEvent.AddListener(NextBeat);
        beat.StepEvent.AddListener(NextStep);

        canStep = false;
        canMove = false;
        canRotate = true;

        direction = Vector2Int.zero;
        rotationIndex = 0;
        isLocked = true;

    }

    void Update(){
        if(isLocked) return;
        lockTime += Time.deltaTime;
        
        board.Clear(this);

        if(canStep) StepToBeat();
        if(canMove) MoveToBeat();
        if(canRotate) getInputForRotate(); //todo: if(Time.time > rotateTime)

        board.Set(this);
    }

    public void NextBeat(){
        canMove = true;
        beats++;
        if (beats > 15) {
            Lock();
            board.Clear(this);
        }
    }

    public void NextStep(){
        canStep = true;
    }

    public void MoveToBeat(){
        if(Input.GetKey(KeyCode.S)){
            canMove = false;
            translate(Vector2Int.left);
        }
        if(Input.GetKey(KeyCode.F)){
            canMove = false;
            translate(Vector2Int.right);
        }

    }

    public void StepToBeat(){
        if(isLocked) return;

        translate(direction);
        if (lockTime >= settings.lockDelay) Lock();
        canStep = false;
    }

    void getInputForRotate(){
        if(Input.GetKeyDown(KeyCode.J)) rotate(-1);
        if(Input.GetKeyDown(KeyCode.K)) rotate(1);
    }

    public void Initialize(Shape shape, Tile tile, Vector3Int position, Vector2Int direction){
        if (!isLocked) return; //only new shape if locked

        this.tile = tile;

        rotationIndex = 0;
        //stepCount = 0;
        isLocked = false;
        enabled = true;
        data = board.shapes[(int)shape];
        this.position = position;
        this.direction = direction;

        lockTime = 0f;
        beats = 0;

        cells = new Vector3Int[data.cells.Length];
        for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    void Lock(){
        //todo: put in message system, for clearing ring
        isLocked = true;
        enabled = false;
        LockEvent.Invoke();
    }

    void translateTiles(Vector2Int translation){
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        position = newPosition;
    }

    bool translate(Vector2Int translation){

        Vector3Int originalPosition = position;
        translateTiles(translation);
        bool isValid = board.IsValidPosition(this, position);
        if(isValid){
            //moveTime = Time.time + settings.moveDelay;
            lockTime = 0f; // reset
        }
        else{
            //stepCount--; //todo: fix hack
            position = originalPosition;
        }
        return isValid;
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
