using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    public int BoardSize = 8;
    public GameObject TilePrefab;
    public GameObject PiecePrefab;

    private const float _pieceUpOffset = 0.5f;
    private const float _lightPieceRowEnd = 3;
    private const int _darkPieceRowStart = 6;

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
                    tileScript.IsDark = true;
                    if (z < _lightPieceRowEnd)
                    {
                        Instantiate(PiecePrefab, new Vector3(x, _pieceUpOffset, z), Quaternion.identity);
                    }
                    else if (z >= _darkPieceRowStart - 1)
                    {
                        GameObject piece = Instantiate(PiecePrefab, new Vector3(x, _pieceUpOffset, z), Quaternion.identity);
                        Piece pieceScript = piece.GetComponent<Piece>();
                        pieceScript.IsDark = true;
                    }
                }
            }
        }
    }
}