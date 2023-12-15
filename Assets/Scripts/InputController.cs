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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, _pieceMask))
            {
                Debug.Log("Piece clicked");
                if (hit.collider.TryGetComponent<Piece>(out var piece))
                {
                    for (int i = 0; i < _markers.Count; i++)
                    {
                        Destroy(_markers[i]);
                    }
                    _markers.Clear();

                    _selectedPiece = piece;
                    var availableMovementsWithCaptures = piece.GetAvailableMovementsWithCaptures();
                    piece.AvailableMovements = availableMovementsWithCaptures.Keys.ToList();
                    _markers = _sceneGenerator.MarkAvailablePositions(piece);
                }
            }
            else
            {
                if (Physics.Raycast(ray, out RaycastHit hit2, 1000, _markerMask))
                {
                    Debug.Log("Marker clicked");
                    if (_selectedPiece.AvailableMovements.Contains(hit2.transform.position))
                    {
                        var capturedPiece = _selectedPiece.GetAvailableMovementsWithCaptures()[hit2.transform.position];
                        if (capturedPiece != null)
                        {
                            Destroy(capturedPiece.gameObject);
                        }
                        _selectedPiece.MoveTo(hit2.transform.position);
                        _selectedPiece = null;
                    }
                }
            }
        }
    }
}
