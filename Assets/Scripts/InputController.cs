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
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, _pieceMask))
            {
                // Piece clicked
                ClearMovementMarkers();
                Piece piece = hit.collider.GetComponent<Piece>();
                piece.MakeMovementMarkers();
            }
            else if (Physics.Raycast(ray, out RaycastHit hit2, 1000, _movementMarkerMask))
            {
                // Movement marker clicked
                ClearMovementMarkers();
                MovementMarker marker = hit2.collider.GetComponent<MovementMarker>();
                marker.SourcePiece.transform.position = marker.transform.position;
                foreach (var piece in marker.CapturablePieces)
                {
                    Destroy(piece.gameObject);
                }
            }
        }
    }

    private void ClearMovementMarkers()
    {
        foreach (var marker in FindObjectsOfType<MovementMarker>())
        {
            Destroy(marker.gameObject);
        }
    }
}
