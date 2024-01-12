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
            boardSize: 5,
            playableTileColor: Color.blue,
            rowsPerTeam: 1,
            darkPieceColor: Color.black,
            startingPieceColor: GameColor.Light,
            flyingKing: true,
            pawnCanCaptureBackwards: false
        );
        //SingleRulesContext.Instance.Rules = BaseRulesStrategyFactory.Create(BaseRulesStrategyType.Brazilian);
        //
        // Get rules
        RulesStrategy rules = SingleRulesContext.Instance.Rules;
        _boardSize = rules.BoardSize;
        _rowsPerTeam = rules.RowsPerTeam;

        // Generate tiles and pieces
        for (int x = 0; x < _boardSize; x++)
        {
            for (int z = 0; z < _boardSize; z++)
            {
                // Instantiate tiles
                Tile tile = Instantiate(_tilePrefab, new Vector3(x, 0, z), Quaternion.identity, _tilesGameObject.transform).GetComponent<Tile>();
                if ((x + z) % 2 == 0)
                {
                    // Dark tiles
                    tile.TileColor = GameColor.Dark;
                    if (z < _rowsPerTeam)
                    {
                        // Instantiate light pieces
                        InstantiatePiece(x, z);
                    }
                    else if (z >= _boardSize - _rowsPerTeam)
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
        return Instantiate(_piecePrefab, new Vector3(x, Utils.PieceUpOffset, z), Quaternion.identity, _piecesGameObject.transform).GetComponent<Piece>();
    }
}