using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    internal PieceType Type;
    internal GameColor PieceColor
    {
        get => _pieceColor;
        set
        {
            _pieceColor = value;
            Rules rules = RulesController.Instance.Get();
            _renderer.material.color = value == GameColor.Light ? Color.white : rules.DarkPieceColor;
        }
    }
    private GameColor _pieceColor;
    internal List<Vector3> AvailableMovements { get; set; }

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    internal List<Vector3> GetAvailableMovements()
    {
        List<Vector3> availableMovements = new List<Vector3>();

        // Define the possible movement directions for a piece
        List<Vector3> directions;
        if (PieceColor == GameColor.Dark)
        {
            // The dark pieces move in the negative z direction
            directions = new List<Vector3> { Vector3.back + Vector3.right, Vector3.back + Vector3.left };
        }
        else
        {
            directions = new List<Vector3> { Vector3.forward + Vector3.right, Vector3.forward + Vector3.left };
        }

        foreach (var direction in directions)
        {
            Vector3 nextPosition = transform.position + direction;

            // Check if the next position is within the board boundaries
            if (IsWithinBoard(nextPosition))
            {
                // Check if the next position is occupied by another piece
                Piece otherPiece = GetPieceAtPosition(nextPosition);
                if (otherPiece != null)
                {
                    // If the other piece is of the opposite color, it can be captured
                    if (otherPiece.PieceColor != PieceColor)
                    {
                        Vector3 capturePosition = nextPosition + direction;

                        // Check if the capture position is within the board and not occupied
                        if (IsWithinBoard(capturePosition) && GetPieceAtPosition(capturePosition) == null)
                        {
                            availableMovements.Add(capturePosition);
                        }
                    }
                }
                else
                {
                    availableMovements.Add(nextPosition);
                }
            }
        }

        return availableMovements;
    }
    internal void MoveTo(Vector3 newPosition)
    {
        if (AvailableMovements.Contains(newPosition))
        {
            transform.position = newPosition;
        }
        else
        {
            Debug.Log("Invalid move");
        }
    }

    private bool IsWithinBoard(Vector3 position)
    {
        Rules rules = RulesController.Instance.Get();
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