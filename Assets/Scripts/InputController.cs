using System.Collections.Generic;
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
                    piece.AvailableMovements = piece.GetAvailableMovements();
                    _markers = _sceneGenerator.MarkAvailablePositions(piece);
                }
            }
            else
            {

                if (Physics.Raycast(ray, out RaycastHit hit2, 1000, _markerMask))
                {
                    Debug.Log("Marker clicked");
                    _selectedPiece.MoveTo(hit2.transform.position);
                    _selectedPiece = null;
                }
            }
        }
    }
}
