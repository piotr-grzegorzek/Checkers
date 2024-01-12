using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleInputController : MonoBehaviour
{
    private static SingleInputController _instance;

    [SerializeField]
    LayerMask _pieceMask;
    [SerializeField]
    LayerMask _movementMarkerMask;

    private GameColor _currentPlayerColor;

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

        //BUG: Pieces dont get killed during victory check, temporarly executing it here
        // On destroy is last in order of execution
        StartCoroutine(CheckVictoryRoutine());
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
                if (piece.PieceColor == _currentPlayerColor)
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

    private IEnumerator CheckVictoryRoutine()
    {
        while (true)
        {
            if (CheckVictory())
            {
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
}
