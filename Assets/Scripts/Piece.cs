using UnityEngine;
using UnityEngine.Tilemaps;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public Vector3Int position { get; private set; }
    public TetrominoData activeTetro { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int rotationIndex { get; private set; }
    
    public void Initialize(Board board,TetrominoData activeTetro,Vector3Int position)
    {
        this.board = board;
        this.position = position;
        this.activeTetro= activeTetro;
        if(this.cells == null)
        {
            this.cells=new Vector3Int[activeTetro.cells.Length];
        }
        for(int i=0;i<this.cells.Length;i++) {
            this.cells[i] = (Vector3Int)activeTetro.cells[i];
        }
    }
    public void Update() 
    {
        this.board.ClearPiece(this);
        if(Input.GetKeyDown(KeyCode.A)) {
            Move(Vector2Int.left);
        }
        if(Input.GetKeyDown(KeyCode.D)) {
            Move(Vector2Int.right);
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            Move(Vector2Int.down);
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            HardDrop();
        }
        if(Input.GetKeyDown(KeyCode.Q)) {
            Rotation(-1);
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            Rotation(1);
        }
        this.board.SetPiece(this);

    }
    public bool Move(Vector2Int translation)
    {
        Vector3Int newPosition=this.position;
        newPosition.x+=translation.x;
        newPosition.y+=translation.y;
        if (this.board.IsOk(newPosition,this))
        {
            this.position=newPosition;
            return true;
        }
        return false;
    } 

    public void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
    }

    public void Rotation(int direction)
    {
        this.rotationIndex = Wrap(direction, 0, 3);

        for(int i=0;i<this.cells.Length;i++)
        {
            Vector3 cell = this.cells[i]; 
            int x, y;
            switch (this.activeTetro.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }
            this.cells[i]= new Vector3Int(x, y,0);
        }
        
    }
    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
