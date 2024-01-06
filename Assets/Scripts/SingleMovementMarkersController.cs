using System.Collections.Generic;
using System.Linq;
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
        ClearMovementMarkers();

        // Get all tiles
        Tile[] tiles = FindObjectsOfType<Tile>();

        // Create markers
        foreach (var tile in tiles)
        {
            List<Piece> capturablePieces = new List<Piece>();

            if (PrepareMovementMarker(tile, piece, tiles, capturablePieces))
            {
                InstantiateMovementMarker(tile, piece, capturablePieces);
            }
        }
    }
    internal void CommitMovementMarker(MovementMarker marker)
    {
        // Move the piece to the marker
        Piece piece = marker.SourcePiece;
        piece.transform.position = marker.transform.position;

        // Capture the pieces
        foreach (var capturablePiece in marker.CapturablePieces)
        {
            Destroy(capturablePiece.gameObject);
        }

        ClearMovementMarkers();
    }

    private void ClearMovementMarkers()
    {
        foreach (var marker in FindObjectsOfType<MovementMarker>())
        {
            Destroy(marker.gameObject);
        }
    }
    private bool PrepareMovementMarker(Tile tile, Piece piece, Tile[] tiles, List<Piece> capturablePieces)
    {
        // Allow only empty tiles
        bool isOccupied = Physics.OverlapSphere(tile.transform.position, 0.1f).Any(collider => collider.TryGetComponent<Piece>(out Piece _));
        if (isOccupied)
        {
            return false;
        }

        // Get the distance between the piece and the tile
        float distance = Vector3.Distance(new Vector3(piece.transform.position.x, 0, piece.transform.position.z), new Vector3(tile.transform.position.x, 0, tile.transform.position.z));

        if (distance == Mathf.Sqrt(2))
        {
            // Single tile movement
            return true;
        }
        else if (distance == 2 * Mathf.Sqrt(2))
        {
            // Jumping over a piece
            // Get the middle tile
            Vector3 middlePoint = Vector3.Lerp(piece.transform.position, tile.transform.position, 0.5f);
            Tile middleTile = tiles.FirstOrDefault(t => (int)t.transform.position.x == (int)middlePoint.x && (int)t.transform.position.z == (int)middlePoint.z);
            if (middleTile != null)
            {
                // Check if the middle tile is occupied by an enemy piece
                Collider[] colliders = Physics.OverlapSphere(middleTile.transform.position, 0.1f);
                Piece middlePiece = null;
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent<Piece>(out Piece tempPiece))
                    {
                        middlePiece = tempPiece;
                        break;
                    }
                }
                if (middlePiece != null && middlePiece.PieceColor != piece.GetComponent<Piece>().PieceColor)
                {
                    // Add the capturable piece to the list
                    capturablePieces.Add(middlePiece);
                    return true;
                }
            }
        }

        return false;
    }
    private void InstantiateMovementMarker(Tile tile, Piece piece, List<Piece> capturablePieces)
    {
        Vector3 offset = new Vector3(0, 0.5f, 0);
        Vector3 newMarkerPosition = tile.transform.position + offset;
        GameObject marker = Instantiate(_movementMarkerPrefab, newMarkerPosition, Quaternion.identity, _movementMarkersGameObject.transform);
        MovementMarker markerScript = marker.GetComponent<MovementMarker>();
        markerScript.SourcePiece = piece;
        markerScript.CapturablePieces = capturablePieces;
    }
}