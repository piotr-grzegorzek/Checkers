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
}
