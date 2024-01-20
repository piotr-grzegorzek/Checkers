using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    LayerMask _pieceMask;
    [SerializeField]
    LayerMask _movementMarkerMask;
    [SerializeField]
    Board _board;

    private const float _clickRayDist = 100f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Mouse clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _clickRayDist, _pieceMask))
            {
                // Piece clicked
                Piece piece = hit.collider.GetComponent<Piece>();
                _board.SelectPiece(piece);
            }
            else if (Physics.Raycast(ray, out RaycastHit hit2, _clickRayDist, _movementMarkerMask))
            {
                // Movement marker clicked
                MovementMarker marker = hit2.collider.GetComponent<MovementMarker>();
                _board.CommitMovementMarker(marker);
            }
        }
    }
}
