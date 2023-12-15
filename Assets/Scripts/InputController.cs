using System.Linq;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    LayerMask _pieceMask;
    [SerializeField]
    LayerMask _markerMask;
    [SerializeField]
    SceneGenerator _sceneGenerator;

    private Piece _selectedPiece;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    private void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, _pieceMask))
        {
            HandlePieceClick(hit);
        }
        else if (Physics.Raycast(ray, out RaycastHit hit2, 1000, _markerMask))
        {
            HandleMarkerClick(hit2);
        }
    }
    private void HandlePieceClick(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<Piece>(out var piece))
        {
            SelectPiece(piece);
        }
    }
    private void SelectPiece(Piece piece)
    {
        _selectedPiece = piece;
        var availableMovementsWithCaptures = piece.GetAvailableMovementsWithCaptures();
        piece.AvailableMovements = availableMovementsWithCaptures.Keys.ToList();
        _sceneGenerator.MarkAvailablePositions(piece);
    }
    private void HandleMarkerClick(RaycastHit hit)
    {
        if (_selectedPiece.AvailableMovements.Contains(hit.transform.position))
        {
            Piece capturedPiece = _selectedPiece.GetAvailableMovementsWithCaptures()[hit.transform.position];
            if (capturedPiece != null)
            {
                Destroy(capturedPiece.gameObject);
            }
            _selectedPiece.MoveTo(hit.transform.position);
            _selectedPiece = null;
        }
    }
}
