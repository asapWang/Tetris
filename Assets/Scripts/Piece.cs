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

}
