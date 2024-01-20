using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    MovementMarkersController _mmc;
    [SerializeField]
    RulesContext _rulesContext;

    internal float PieceUpOffset
    {
        get => _pieceUpOffset;
        // Set only once
        set => _pieceUpOffset = _pieceUpOffset == 0 ? value : _pieceUpOffset;
    }
    private float _pieceUpOffset;
    internal float PieceCheckRadius
    {
        get => _pieceCheckRadius;
        set => _pieceCheckRadius = _pieceCheckRadius == 0 ? value : _pieceCheckRadius;
    }
    private float _pieceCheckRadius;
    internal float SingleTileDistance
    {
        get => _singleTileDistance;
        set => _singleTileDistance = _singleTileDistance == 0 ? value : _singleTileDistance;
    }
    private float _singleTileDistance;
    internal float JumpDistance
    {
        get => _jumpDistance;
        set => _jumpDistance = _jumpDistance == 0 ? value : _jumpDistance;
    }
    private float _jumpDistance;

    private GameColor _currentPlayerColor;
    private bool _gameRunning = true;
    private int _lightPlayerKingMovesWithoutCapture = 0;
    private int _darkPlayerKingMovesWithoutCapture = 0;

    void Start()
    {
        _currentPlayerColor = _rulesContext.Rules.StartingPieceColor;

        //BUG: Pieces dont get killed during victory check, temporarly executing it here
        // On destroy is last in order of execution
        StartCoroutine(StartCheckGameStageRoutineAfterDelay());
    }

    internal void SelectPiece(Piece piece)
    {
        // Check if it's the current player's piece
        if (piece.PieceColor == _currentPlayerColor && _gameRunning)
        {
            _mmc.MakeMovementMarkers(piece);
        }
    }
    internal IEnumerable<Piece> GetPiecesOfColor(GameColor color)
    {
        return FindObjectsOfType<Piece>().Where(p => p.PieceColor == color);
    }
    internal void RegisterKingMoveWithoutCapture(GameColor color)
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
    internal void RegisterPieceCapture()
    {
        _lightPlayerKingMovesWithoutCapture = 0;
        _darkPlayerKingMovesWithoutCapture = 0;
    }
    internal void CommitMovementMarker(MovementMarker marker)
    {
        _mmc.CommitMovementMarker(marker);
        EndTurn();
        CheckVictory();
    }

    private IEnumerator StartCheckGameStageRoutineAfterDelay()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(CheckGameStageRoutine());
    }
    private IEnumerator CheckGameStageRoutine()
    {
        while (true)
        {
            if (Victory() || Draw())
            {
                _gameRunning = false;
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private bool Victory()
    {
        IEnumerable<Piece> pieces = GetPiecesOfColor(_currentPlayerColor);
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
    private bool Draw()
    {
        if (_lightPlayerKingMovesWithoutCapture >= 15 && _darkPlayerKingMovesWithoutCapture >= 15)
        {
            // Draw
            Debug.Log("Draw");
            return true;
        }
        return false;
    }
    private void EndTurn()
    {
        _currentPlayerColor = _currentPlayerColor == GameColor.Light ? GameColor.Dark : GameColor.Light;
        Debug.Log($"Player {_currentPlayerColor} turn");
    }
    private void CheckVictory()
    {
        //TODO
    }
}