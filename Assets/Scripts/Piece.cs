using UnityEngine;

public class Piece : MonoBehaviour
{
    public ShapeData data;
    public Vector3Int[] cells;
    public Vector3Int position;
    public Vector2Int direction;
    public int rotationIndex;
    public bool isLocked {get; private set;}

    public Game game;
    public Board board;

    public bool MoveLeft;
    public bool MoveRight;
    public bool MoveDown;
    public bool RotateLeft;
    public bool RotateRight;
    
    private float stepTime;
    private float moveTime;
    private float lockTime;

    void Awake(){
        game = GameObject.Find("Game").GetComponent<Game>();
        board = GameObject.Find("Board").GetComponent<Board>();
        rotationIndex = 0;
        isLocked = false;
        direction = Vector2Int.zero;
    }

    void Update(){
        board.Clear(this);
        
        lockTime += Time.deltaTime;
        if (Time.time > stepTime) step();

        bool isMoveable = Time.time > moveTime && !isLocked;
        if(isMoveable){
            if(MoveLeft)Move(Vector2Int.left);
            if(MoveRight)Move(Vector2Int.right);
            if(MoveDown){
                stepTime = Time.time + game.settings.stepDelay;
                Move(Vector2Int.down);
            }

            if(RotateLeft)Rotate(-1);
            if(RotateRight)Rotate(1);
        }
        
        MoveLeft = false;
        MoveRight = false;
        MoveDown = false;
        RotateLeft = false;
        RotateRight = false;
        
        board.Set(this);
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

    private void step(){
        stepTime = Time.time + game.settings.stepDelay;
        Move(direction);
        if (lockTime >= game.settings.lockDelay) Lock();
    }

    public void Lock(){
        board.Set(this);
        isLocked = true;
        //board.ClearLines();
    }

    private void translateTiles(Vector2Int translation){
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        position = newPosition;
    }

    private bool Move(Vector2Int translation){

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

    private void rotateTiles(int direction){
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

    private void Rotate(int direction){
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

    private bool tryMove(Vector2Int translation){
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        return board.IsValidPosition(this, newPosition);
    }

    private bool tryWallKick(int rotationDirection){
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

    private int getWallKickIndex(int rotationIndex, int rotationDirection){
        int wallKickIndex = rotationIndex * 2;
        if (rotationDirection < 0) wallKickIndex--;
        return wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }

    private int wrap(int input, int min, int max){
        if (input < min) return max - (min - input) % (max - min);
        return min + (input - min) % (max - min);
    }

}
