using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    LayerMask _pieceMask;
    [SerializeField]
    LayerMask _movementMarkerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Mouse clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, _pieceMask))
            {
                // Piece clicked
                SingleMovementMarkersController mmc = SingleMovementMarkersController.Instance;
                mmc.ClearMovementMarkers();
                Piece piece = hit.collider.GetComponent<Piece>();
                mmc.MakeMovementMarkers(piece);
            }
            else if (Physics.Raycast(ray, out RaycastHit hit2, 100, _movementMarkerMask))
            {
                // Movement marker clicked
                SingleMovementMarkersController mmc = SingleMovementMarkersController.Instance;
                mmc.ClearMovementMarkers();
                MovementMarker marker = hit2.collider.GetComponent<MovementMarker>();
                marker.SourcePiece.transform.position = marker.transform.position;
                foreach (var piece in marker.CapturablePieces)
                {
                    Destroy(piece.gameObject);
                }
            }
        }
    }
}
