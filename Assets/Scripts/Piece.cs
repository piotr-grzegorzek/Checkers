using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField]
    GameObject _movementMarkerPrefab;

    internal PieceType Type;
    internal GameColor PieceColor
    {
        get => _pieceColor;
        set
        {
            _pieceColor = value;
            Rules rules = RulesController.Instance.Rules;
            _renderer.material.color = value == GameColor.Light ? Color.white : rules.DarkPieceColor;
        }
    }
    private GameColor _pieceColor;

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    internal void MakeMovementMarkers()
    {
        // Define the possible movement directions for a piece
        List<Vector3> directions;
        if (PieceColor == GameColor.Dark)
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
            Vector3 nextPosition = transform.position + direction;

            if (IsWithinBoard(nextPosition))
            {
                Piece otherPiece = GetPieceAtPosition(nextPosition);
                if (otherPiece != null)
                {
                    if (otherPiece.PieceColor != PieceColor)
                    {
                        Vector3 capturePosition = nextPosition + direction;

                        if (IsWithinBoard(capturePosition) && GetPieceAtPosition(capturePosition) == null)
                        {
                            GameObject marker = Instantiate(_movementMarkerPrefab, capturePosition, Quaternion.identity);
                            MovementMarker markerScript = marker.GetComponent<MovementMarker>();
                            markerScript.SourcePiece = this;
                            markerScript.CapturablePieces = new List<Piece> { otherPiece };
                        }
                    }
                }
                else
                {
                    GameObject marker = Instantiate(_movementMarkerPrefab, nextPosition, Quaternion.identity);
                    MovementMarker markerScript = marker.GetComponent<MovementMarker>();
                    markerScript.SourcePiece = this;
                    markerScript.CapturablePieces = new List<Piece>();
                }
            }
        }
    }

    private bool IsWithinBoard(Vector3 position)
    {
        Rules rules = RulesController.Instance.Rules;
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