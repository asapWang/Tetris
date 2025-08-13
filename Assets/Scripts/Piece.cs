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
        int oldRotationIndex=this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex+direction, 0, 4);
        ApplyRotate(direction);
        if(!KickWallsTest(oldRotationIndex,direction))
        {
            ApplyRotate(-direction);  
            this.rotationIndex=oldRotationIndex;
        }
 
    }

    public void ApplyRotate(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
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
            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    public bool KickWallsTest(int rotationIndex,int direction)
    {
        int wallKicksIndex=GetWallKicksIndex(rotationIndex, direction);
        for(int i=0;i<this.activeTetro.wallkicks.GetLength(1);i++)
        {
            Vector2Int translation = this.activeTetro.wallkicks[wallKicksIndex,i];
            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    public int GetWallKicksIndex(int rotationIndex, int direction)
    {
        int wallKicksIndex = rotationIndex * 2;
        if(direction<0)
        {
            wallKicksIndex--;
        }
        wallKicksIndex=Wrap(wallKicksIndex,0,this.activeTetro.wallkicks.GetLength(0));
        return wallKicksIndex;
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
