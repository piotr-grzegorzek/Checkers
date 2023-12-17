using System.Collections.Generic;
using UnityEngine;

public class SingleMovementMarkersController : MonoBehaviour
{
    internal static SingleMovementMarkersController Instance { get; private set; }

    [SerializeField]
    GameObject _movementMarkerPrefab;
    [SerializeField]
    GameObject _movementMarkersGameObject;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    internal void MakeMovementMarkers(Piece piece)
    {
        // Define the possible movement directions for a piece
        List<Vector3> directions;
        if (piece.PieceColor == GameColor.Dark)
        {
            // The dark pieces move in the negative z direction
            directions = new List<Vector3> { Vector3.back + Vector3.right, Vector3.back + Vector3.left };
        }
        else
        {
            // The light pieces move in the positive z direction
            directions = new List<Vector3> { Vector3.forward + Vector3.right, Vector3.forward + Vector3.left };
        }

        foreach (var direction in directions)
        {
            Vector3 nextPosition = piece.transform.position + direction;

            if (IsWithinBoard(nextPosition))
            {
                Piece otherPiece = GetPieceAtPosition(nextPosition);
                if (otherPiece != null)
                {
                    if (otherPiece.PieceColor != piece.PieceColor)
                    {
                        Vector3 capturePosition = nextPosition + direction;

                        if (IsWithinBoard(capturePosition) && GetPieceAtPosition(capturePosition) == null)
                        {
                            GameObject marker = Instantiate(_movementMarkerPrefab, capturePosition, Quaternion.identity, _movementMarkersGameObject.transform);
                            MovementMarker markerScript = marker.GetComponent<MovementMarker>();
                            markerScript.SourcePiece = piece;
                            markerScript.CapturablePieces = new List<Piece> { otherPiece };
                        }
                    }
                }
                else
                {
                    GameObject marker = Instantiate(_movementMarkerPrefab, nextPosition, Quaternion.identity, _movementMarkersGameObject.transform);
                    MovementMarker markerScript = marker.GetComponent<MovementMarker>();
                    markerScript.SourcePiece = piece;
                    markerScript.CapturablePieces = new List<Piece>();
                }
            }
        }
    }
    internal void ClearMovementMarkers()
    {
        foreach (var marker in FindObjectsOfType<MovementMarker>())
        {
            Destroy(marker.gameObject);
        }
    }

    private bool IsWithinBoard(Vector3 position)
    {
        RulesStrategy rules = SingleRulesStrategyController.Instance.Rules;
        return position.x >= 0 && position.x < rules.BoardSize && position.z >= 0 && position.z < rules.BoardSize;
    }
    private Piece GetPieceAtPosition(Vector3 position)
    {
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out RaycastHit hit, 2))
        {
            return hit.collider.GetComponent<Piece>();
        }
        return null;
    }
}