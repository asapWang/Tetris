using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostBoard : MonoBehaviour
{
    public Board board;
    public Piece trackingPiece;
    public Tile tile;
    public Tilemap tilemap { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set;}

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    public void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    public void Clear()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void Copy()
    {
        this.position=this.trackingPiece.position;
        for (int i = 0; i < 4; i++)
        {
            this.cells[i] = this.trackingPiece.cells[i];
        }
    }

    public void Drop()
    {
        this.board.ClearPiece(this.trackingPiece);
        while (Move(Vector2Int.down))
        {
            continue;
        }
        this.board.SetPiece(this.trackingPiece);
    }

    public void Set()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }

    public bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        if (IsOk(newPosition))
        {
            this.position = newPosition;
            return true;
        }
        return false;
    }

    public bool IsOk(Vector3Int newPosition)
    {

        for (int i = 0; i < 4; i++)
        {
            Vector3Int tilePosition = newPosition + this.cells[i];
            if (this.board.tilemap.HasTile(tilePosition))
            {
                return false;
            }
            if (!this.board.boundary.Contains((Vector2Int)tilePosition))
            {
                return false;
            }
        }
        return true;
    }
}
