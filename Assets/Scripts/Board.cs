using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Piece piece { get; private set; }
    public Vector3Int position;
    public TetrominoData[] tetrominoDatas;
    public Tilemap tilemap { get; private set; }
    private void Awake()
    {
        this.piece = GetComponentInChildren<Piece>();
        this.tilemap = GetComponentInChildren<Tilemap>();
        for(int i=0;i<tetrominoDatas.Length;i++)
        {
            this.tetrominoDatas[i].Initialize();
        }
    }
    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoDatas.Length);
        TetrominoData tetrominoData = this.tetrominoDatas[random];
        this.piece.Initialize(this, tetrominoData, position);
        SetPiece(this.piece);
    }

    public void SetPiece(Piece piece)
    {
        for(int i=0;i<piece.cells.Length;i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.activeTetro.tile);
        }
       
    }

}
