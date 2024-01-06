using UnityEngine;

public class SingleStartBoardGenerator : MonoBehaviour
{
    private static SingleStartBoardGenerator _instance;

    [SerializeField]
    GameObject _tilePrefab;
    [SerializeField]
    GameObject _piecePrefab;
    [SerializeField]
    GameObject _tilesGameObject;
    [SerializeField]
    GameObject _piecesGameObject;

    private int _boardSize;
    private int _rowsPerTeam;
    private const float _pieceUpOffset = 0.5f;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //TEMP
        SingleRulesContext.Instance.Rules = CustomRulesStrategyFactory.Create(
            boardSize: 7,
            playableTileColor: Color.blue,
            rowsPerTeam: 2,
            darkPieceColor: Color.black,
            startingPieceColor: GameColor.Light
        );
        SingleRulesContext.Instance.Rules = BaseRulesStrategyFactory.Create(BaseRulesStrategyType.Brazilian);
        //
        RulesStrategy rules = SingleRulesContext.Instance.Rules;
        _boardSize = rules.BoardSize;
        _rowsPerTeam = rules.RowsPerTeam;
        GenerateBoard();
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
        GameObject tile = Instantiate(_tilePrefab, new Vector3(x, 0, z), Quaternion.identity, _tilesGameObject.transform);
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
        return Instantiate(_piecePrefab, new Vector3(x, _pieceUpOffset, z), Quaternion.identity, _piecesGameObject.transform);
    }
    private void SetPieceColor(GameObject piece, GameColor color)
    {
        Piece pieceScript = piece.GetComponent<Piece>();
        pieceScript.PieceColor = color;
    }
}