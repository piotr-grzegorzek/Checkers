using System.Collections.Generic;
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
    private List<GameObject> _markers = new List<GameObject>();

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
            ClearMarkers();
            SelectPiece(piece);
        }
    }
    private void ClearMarkers()
    {
        for (int i = 0; i < _markers.Count; i++)
        {
            Destroy(_markers[i]);
        }
        _markers.Clear();
    }
    private void SelectPiece(Piece piece)
    {
        _selectedPiece = piece;
        var availableMovementsWithCaptures = piece.GetAvailableMovementsWithCaptures();
        piece.AvailableMovements = availableMovementsWithCaptures.Keys.ToList();
        _markers = _sceneGenerator.MarkAvailablePositions(piece);
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
