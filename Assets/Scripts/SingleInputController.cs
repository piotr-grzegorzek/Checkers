using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleInputController : MonoBehaviour
{
    private static SingleInputController _instance;
    private static int _lightPlayerKingMovesWithoutCapture = 0;
    private static int _darkPlayerKingMovesWithoutCapture = 0;

    [SerializeField]
    LayerMask _pieceMask;
    [SerializeField]
    LayerMask _movementMarkerMask;

    private GameColor _currentPlayerColor;
    private bool _gameRunning;

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
        RulesStrategy rules = SingleRulesContext.Instance.Rules;
        _currentPlayerColor = rules.StartingPieceColor;
        _gameRunning = true;

        //BUG: Pieces dont get killed during victory check, temporarly executing it here
        // On destroy is last in order of execution
        StartCoroutine(CheckGameStageRoutine());
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Mouse clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, _pieceMask))
            {
                // Piece clicked
                Piece piece = hit.collider.GetComponent<Piece>();
                // Check if it's the current player's piece
                if (piece.PieceColor == _currentPlayerColor && _gameRunning)
                {
                    SingleMovementMarkersController mmc = SingleMovementMarkersController.Instance;
                    mmc.MakeMovementMarkers(piece);
                }
            }
            else if (Physics.Raycast(ray, out RaycastHit hit2, 100, _movementMarkerMask))
            {
                // Marker clicked
                MovementMarker marker = hit2.collider.GetComponent<MovementMarker>();
                SingleMovementMarkersController mmc = SingleMovementMarkersController.Instance;
                mmc.CommitMovementMarker(marker);
                // End turn
                _currentPlayerColor = _currentPlayerColor == GameColor.Light ? GameColor.Dark : GameColor.Light;
                Debug.Log($"Player {_currentPlayerColor} turn");
                // Check victory
            }
        }
    }

    internal static void HandleKingMoveWithoutCapture(GameColor color)
    {
        if (color == GameColor.Light)
        {
            _lightPlayerKingMovesWithoutCapture++;
        }
        else
        {
            _darkPlayerKingMovesWithoutCapture++;
        }
    }
    internal static void HandlePieceCapture()
    {
        _lightPlayerKingMovesWithoutCapture = 0;
        _darkPlayerKingMovesWithoutCapture = 0;
    }

    private IEnumerator CheckGameStageRoutine()
    {
        while (true)
        {
            if (CheckVictory() || CheckDraw())
            {
                _gameRunning = false;
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private bool CheckVictory()
    {
        IEnumerable<Piece> pieces = Utils.GetPiecesOfColor(_currentPlayerColor);
        // Check if the attacked player got last piece killed
        if (!pieces.Any())
        {
            // Victory
            GameColor wonColor = _currentPlayerColor == GameColor.Light ? GameColor.Dark : GameColor.Light;
            Debug.Log($"{wonColor} won");
            return true;
        }
        return false;
    }
    private bool CheckDraw()
    {
        if (_lightPlayerKingMovesWithoutCapture >= 15 && _darkPlayerKingMovesWithoutCapture >= 15)
        {
            // Draw
            Debug.Log("Draw");
            return true;
        }
        return false;
    }
}
