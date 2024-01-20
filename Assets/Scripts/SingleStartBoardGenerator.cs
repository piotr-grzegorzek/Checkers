using UnityEngine;

public class SingleStartBoardGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject _tilePrefab;
    [SerializeField]
    GameObject _piecePrefab;
    [SerializeField]
    GameObject _tilesGameObject;
    [SerializeField]
    GameObject _piecesGameObject;
    [SerializeField]
    Board _board;
    [SerializeField]
    RulesContext _rulesContext;

    private static SingleStartBoardGenerator _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Temp();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        InitializeBoardParameters();
        GenerateTilesAndPieces();
    }

    private void InitializeBoardParameters()
    {
        _board.PieceUpOffset = _piecePrefab.GetComponent<Renderer>().bounds.size.y / 2;
        _board.PieceCheckRadius = _board.PieceUpOffset;

        float tileSize = _tilePrefab.GetComponent<Renderer>().bounds.size.x;
        // Set the SingleTileDistance based on the tile size
        _board.SingleTileDistance = Mathf.Sqrt(tileSize * tileSize + tileSize * tileSize);
        _board.JumpDistance = _board.SingleTileDistance * 2;
    }
    private void GenerateTilesAndPieces()
    {
        // Get rules
        RulesStrategy rules = _rulesContext.Rules;
        for (int x = 0; x < rules.BoardSize; x++)
        {
            for (int z = 0; z < rules.BoardSize; z++)
            {
                // Instantiate tiles
                Tile tile = Instantiate(_tilePrefab, new Vector3(x, 0, z), Quaternion.identity, _tilesGameObject.transform).GetComponent<Tile>();
                if ((x + z) % 2 == 0)
                {
                    // Dark tiles
                    tile.TileColor = GameColor.Dark;
                    if (z < rules.RowsPerTeam)
                    {
                        // Instantiate light pieces
                        InstantiatePiece(x, z);
                    }
                    else if (z >= rules.BoardSize - rules.RowsPerTeam)
                    {
                        // Instantiate dark pieces
                        Piece piece = InstantiatePiece(x, z);
                        piece.PieceColor = GameColor.Dark;
                    }
                }
            }
        }
    }
    private Piece InstantiatePiece(int x, int z)
    {
        return Instantiate(_piecePrefab, new Vector3(x, _board.PieceUpOffset, z), Quaternion.identity, _piecesGameObject.transform).GetComponent<Piece>();
    }

    private void Temp()
    {
        _rulesContext.Rules = CustomRulesStrategyFactory.Create(
            boardSize: 5,
            playableTileColor: Color.blue,
            rowsPerTeam: 1,
            darkPieceColor: Color.black,
            startingPieceColor: GameColor.Light,
            flyingKing: true,
            pawnCanCaptureBackwards: false
        );
        //_rulesContext.Rules = BaseRulesStrategyFactory.Create(BaseRulesStrategyType.Brazilian);
    }
}