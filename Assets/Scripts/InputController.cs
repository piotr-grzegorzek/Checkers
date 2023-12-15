using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    LayerMask _pieceMask;
    [SerializeField]
    SceneGenerator _sceneGenerator;

    private Piece _selectedPiece;
    private List<GameObject> _markers = new List<GameObject>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 123, _pieceMask))
            {
                if (hit.collider.TryGetComponent<Piece>(out var piece))
                {
                    // If a new piece is selected, destroy the existing markers
                    if (_selectedPiece != piece)
                    {
                        foreach (var marker in _markers)
                        {
                            Destroy(marker);
                        }
                        _markers.Clear();
                    }

                    _selectedPiece = piece;
                    piece.AvailableMovements = piece.GetAvailableMovements();
                    _markers = _sceneGenerator.MarkAvailablePositions(piece);
                    Debug.Log($"Available movements for {piece.Type} at {piece.transform.position}: {string.Join(", ", piece.AvailableMovements)}");
                }
            }
        }
    }
}
