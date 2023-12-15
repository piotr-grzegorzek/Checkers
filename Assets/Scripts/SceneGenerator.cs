using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject _tilePrefab;
    [SerializeField]
    GameObject _piecePrefab;

    private int _boardSize;
    private int _rowsPerTeam;
    private const float _pieceUpOffset = 0.5f;

    void Start()
    {
        Rules rules = RulesController.Instance.Get();
        _boardSize = rules.BoardSize;
        _rowsPerTeam = rules.RowsPerTeam;
        GenerateBoard();
        Destroy(gameObject);
    }

    private void GenerateBoard()
    {
        for (int x = 0; x < _boardSize; x++)
        {
            for (int z = 0; z < _boardSize; z++)
            {
                GenerateTile(x, z);
            }
        }
    }
    private void GenerateTile(int x, int z)
    {
        GameObject tile = Instantiate(_tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
        if ((x + z) % 2 == 0)
        {
            SetTileColor(tile, GameColor.Dark);
            InstantiatePieceIfNeeded(x, z);
        }
    }
    private void SetTileColor(GameObject tile, GameColor color)
    {
        Tile tileScript = tile.GetComponent<Tile>();
        tileScript.TileColor = color;
    }
    private void InstantiatePieceIfNeeded(int x, int z)
    {
        if (z < _rowsPerTeam)
        {
            InstantiatePiece(x, z);
        }
        else if (z >= _boardSize - _rowsPerTeam)
        {
            GameObject piece = InstantiatePiece(x, z);
            SetPieceColor(piece, GameColor.Dark);
        }
    }
    private GameObject InstantiatePiece(int x, int z)
    {
        return Instantiate(_piecePrefab, new Vector3(x, _pieceUpOffset, z), Quaternion.identity);
    }
    private void SetPieceColor(GameObject piece, GameColor color)
    {
        Piece pieceScript = piece.GetComponent<Piece>();
        pieceScript.PieceColor = color;
    }
}