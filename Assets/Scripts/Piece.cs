using UnityEngine;
using UnityEngine.Tilemaps;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public Vector3Int position { get; private set; }
    public TetrominoData activeTetro { get; private set; }
    public Vector3Int[] cells { get; private set; }
    
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

}
