using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    public int BoardSize = 8;
    public int RowsPerTeam = 3;
    public GameObject TilePrefab;
    public GameObject PiecePrefab;

    private const float _pieceUpOffset = 0.5f;

    void Start()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int z = 0; z < BoardSize; z++)
            {
                GameObject tile = Instantiate(TilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                if ((x + z) % 2 == 0)
                {
                    Tile tileScript = tile.GetComponent<Tile>();
                    tileScript.TileColor = GameColor.Dark;
                    if (z < RowsPerTeam)
                    {
                        InstantiatePiece(x, z);
                    }
                    else if (z >= BoardSize - RowsPerTeam)
                    {
                        GameObject piece = InstantiatePiece(x, z);
                        Piece pieceScript = piece.GetComponent<Piece>();
                        pieceScript.PieceColor = GameColor.Dark;
                    }
                }
            }
        }
    }

    private GameObject InstantiatePiece(int x, int z)
    {
        return Instantiate(PiecePrefab, new Vector3(x, _pieceUpOffset, z), Quaternion.identity);
    }
}