using UnityEngine;

public class Piece : MonoBehaviour{
    public ShapeData data;
    public Vector3Int[] cells;
    public Vector3Int position;
    public Vector2Int direction;
    public int rotationIndex;
    public bool isLocked;

    public Game game;
    public Board board;
    public Core core;

    float stepTime;
    private float moveTime;
    private float lockTime;

    void Awake(){
        game = GameObject.Find("Game").GetComponent<Game>();
        board = GameObject.Find("Board").GetComponent<Board>();
        core = GameObject.Find("Core").GetComponent<Core>();

        rotationIndex = 0;
        isLocked = false;
        direction = Vector2Int.zero;
    }

    void Update(){
        if (isLocked) return;

        board.Clear(this);
        
        lockTime += Time.deltaTime;
        if (Time.time > stepTime) step();
        if(Time.time > moveTime) handleMovement();

        board.Set(this);
        if (isLocked) core.SetColors();
    }

    void step(){
        stepTime = Time.time + game.settings.stepDelay;
        Move(direction);
        if (lockTime >= game.settings.lockDelay) Lock();
    }

    void handleMovement(){
        if(Input.GetKeyDown(KeyCode.S))Move(Vector2Int.left);
        if(Input.GetKeyDown(KeyCode.F) )Move(Vector2Int.right);

        if(Input.GetKeyDown(KeyCode.D)){
            stepTime = Time.time + game.settings.stepDelay;
            Move(Vector2Int.down);
        }
        if(Input.GetKeyDown(KeyCode.J)) Rotate(-1);
        if(Input.GetKeyDown(KeyCode.K)) Rotate(1);
    }

    public void Initialize(Shape shape, Vector3Int position, Vector2Int direction){
        isLocked = false;
        data = board.shapes[(int)shape];
        this.position = position;
        this.direction = direction;

        stepTime = Time.time + game.settings.stepDelay;
        moveTime = Time.time + game.settings.moveDelay;
        lockTime = 0f;

        cells = new Vector3Int[data.cells.Length];
        for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    void Lock(){
        core.SetColors();
        isLocked = true;
        
        //todo: clear ring here
    }

    void translateTiles(Vector2Int translation){
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        position = newPosition;
    }

    bool Move(Vector2Int translation){

        Vector3Int originalPosition = position;
        translateTiles(translation);
        bool isValid = board.IsValidPosition(this, position);
        if(isValid){
            moveTime = Time.time + game.settings.moveDelay;
            lockTime = 0f; // reset
        }
        else{
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

    void Rotate(int direction){
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
                Move(translation);
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
