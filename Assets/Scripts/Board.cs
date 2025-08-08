using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Piece piece { get; private set; }
    public Vector3Int position;
    public TetrominoData[] tetrominoDatas;
    public Tilemap tilemap { get; private set; }
    private Vector2Int boundarySize = new Vector2Int(10, 20);
    private RectInt boundary
    {
        get
        {
            Vector2Int position=new Vector2Int(-boundarySize.x/2,-boundarySize.y/2);
            return new RectInt(position, boundarySize);
        }
    }

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

    public void ClearPiece(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }

    }

    public bool IsOk(Vector3Int newPosition,Piece piece)
    {

        for(int i=0;i< piece.cells.Length;i++)
        {
            Vector3Int tilePosition = newPosition + piece.cells[i];
            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
            if (!this.boundary.Contains((Vector2Int)tilePosition))
            {
                return false;
            }
        }
        return true;
    }
}
