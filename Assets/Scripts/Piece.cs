using UnityEngine;

public class Piece : MonoBehaviour{
    public ShapeData data;
    public Vector3Int[] cells;
    public Vector3Int position;
    public Vector2Int direction;
    public int rotationIndex;

    public Game game;
    public Board board;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    void Awake(){
        game = GameObject.Find("Game").GetComponent<Game>();
        board = GameObject.Find("Board").GetComponent<Board>();
        rotationIndex = 0;
        direction = Vector2Int.zero;
    }

    public void Initialize(Shape shape, Vector3Int position, Vector2Int direction){
        data = board.shapes[(int)shape];
        this.position = position;
        this.direction = direction;

        stepTime = Time.time + game.Settings.stepDelay;
        moveTime = Time.time + game.Settings.moveDelay;
        lockTime = 0f;

        cells = new Vector3Int[data.cells.Length];
        for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector3Int)data.cells[i];
        }
        game.Board.Set(this);
    }

    void Update(){
        game.Board.Clear(this);

        lockTime += Time.deltaTime;
        // if (Time.time > moveTime) HandleMoveInputs();
        if (Time.time > stepTime) Step();

        game.Board.Set(this);
    }

    private void Step(){
        stepTime = Time.time + game.Settings.stepDelay;
        Move(direction);
        if (lockTime >= game.Settings.lockDelay) Lock();
    }

    public void Lock()
    {
        board.Set(this);
        //board.ClearLines();
    }

    private bool Move(Vector2Int translation){
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        // Only save the movement if the new position is valid
        if (valid)
        {
            position = newPosition;
            moveTime = Time.time + game.Settings.moveDelay;
            lockTime = 0f; // reset
        }
        return valid;
    }

        private void Rotate(int direction){
        float[] matrix = Data.RotationMatrix;

        // Rotate all of the cells using the rotation matrix
        for (int i = 0; i < cells.Length; i++){
            Vector3 cell = cells[i];
            int x, y;

            switch (data.shape){
                case Shape.I:
                case Shape.O:
                    // "I" and "O" are rotated from an offset center point
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

    // private void HardDrop()
    // {
    //     while (Move(player.FallDirection)) {
    //         continue;
    //     }
    //     Lock();
    // }

    // private void HandleMoveInputs()
    // {
    //     // Soft drop movement
    //     if (Input.GetKey(KeyCode.S))
    //     {
    //         Move(Vector2Int.down);
    //             // Update the step time to prevent double movement
    //         stepTime = Time.time + stepDelay;
    //     }

    //     // Left/right movement
    //     if (Input.GetKey(KeyCode.A)) {
    //         Move(Vector2Int.left);
    //     } else if (Input.GetKey(KeyCode.D)) {
    //         Move(Vector2Int.right);
    //     }
    // }

    // private void Rotate(int direction){
    //     // Store the current rotation in case the rotation fails
    //     // and we need to revert
    //     int originalRotation = rotationIndex;

    //     // Rotate all of the cells using a rotation matrix
    //     rotationIndex = Wrap(rotationIndex + direction, 0, 4);
    //     ApplyRotationMatrix(direction);

    //     // Revert the rotation if the wall kick tests fail
    //     if (!TestWallKicks(rotationIndex, direction))
    //     {
    //         rotationIndex = originalRotation;
    //         ApplyRotationMatrix(-direction);
    //     }
    // }



    // private bool TestWallKicks(int rotationIndex, int rotationDirection){
    //     int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

    //     for (int i = 0; i < data.wallKicks.GetLength(1); i++)
    //     {
    //         Vector2Int translation = data.wallKicks[wallKickIndex, i];

    //         if (Move(translation)) {
    //             return true;
    //         }
    //     }

    //     return false;
    // }

    // private int GetWallKickIndex(int rotationIndex, int rotationDirection){
    //     int wallKickIndex = rotationIndex * 2;

    //     if (rotationDirection < 0) {
    //         wallKickIndex--;
    //     }

    //     return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    // }

    // private int Wrap(int input, int min, int max){
    //     if (input < min) {
    //         return max - (min - input) % (max - min);
    //     } else {
    //         return min + (input - min) % (max - min);
    //     }
    // }

}
