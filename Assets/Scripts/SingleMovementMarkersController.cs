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

        // Get pieces from the current player
        IEnumerable<Piece> currentPlayerPieces = FindObjectsOfType<Piece>().Where(p => p.PieceColor == piece.PieceColor);
        //Get dark tiles
        IEnumerable<Tile> darkTiles = FindObjectsOfType<Tile>().Where(t => t.TileColor == GameColor.Dark);

        // Prepare movement markers
        List<PreparedMovementMarker> preparedMovementMarkers = new List<PreparedMovementMarker>();
        foreach (var currentPlayerPiece in currentPlayerPieces)
        {
            foreach (var darkTile in darkTiles)
            {
                List<Piece> capturablePieces = new List<Piece>();
                if (PrepareMovementMarker(darkTile, currentPlayerPiece, darkTiles, capturablePieces))
                {
                    preparedMovementMarkers.Add(new PreparedMovementMarker
                    {
                        Tile = darkTile,
                        Piece = currentPlayerPiece,
                        CapturablePieces = capturablePieces
                    });
                }
            }
        }

        if (preparedMovementMarkers.Any(m => m.CapturablePieces.Count > 0))
        {
            // Instantiate movement markers with capturable pieces > 0 and Piece = piece only
            foreach (var preparedMovementMarker in preparedMovementMarkers.Where(m => m.CapturablePieces.Count > 0 && m.Piece == piece))
            {
                InstantiateMovementMarker(preparedMovementMarker.Tile, preparedMovementMarker.Piece, preparedMovementMarker.CapturablePieces);
            }
        }
        else
        {
            // Instantiate movement markers with Piece = piece only
            foreach (var preparedMovementMarker in preparedMovementMarkers.Where(m => m.Piece == piece))
            {
                InstantiateMovementMarker(preparedMovementMarker.Tile, preparedMovementMarker.Piece, preparedMovementMarker.CapturablePieces);
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
    private bool PrepareMovementMarker(Tile tile, Piece piece, IEnumerable<Tile> darkTiles, List<Piece> capturablePieces)
    {
        // Allow only empty tiles
        bool isOccupied = GetPieceFromCollider(Physics.OverlapSphere(tile.transform.position, 0.1f)) != null;
        if (isOccupied)
        {
            return false;
        }
        
        // Get the distance between the piece and the tile
        float distance = Vector3.Distance(
            new Vector3(piece.transform.position.x, 0, piece.transform.position.z),
            new Vector3(tile.transform.position.x, 0, tile.transform.position.z));

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
            Tile middleTile = darkTiles.FirstOrDefault(t =>
                (int)t.transform.position.x == (int)middlePoint.x &&
                (int)t.transform.position.z == (int)middlePoint.z);

            if (middleTile != null)
            {
                // Check if the middle tile is occupied by an enemy piece
                Piece middlePiece = GetPieceFromCollider(Physics.OverlapSphere(middleTile.transform.position, 0.1f));

                if (middlePiece != null && middlePiece.PieceColor != piece.PieceColor)
                {
                    // Add the capturable piece to the list
                    capturablePieces.Add(middlePiece);
                    return true;
                }
            }
        }

        return false;
    }

    private Piece GetPieceFromCollider(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Piece>(out Piece tempPiece))
            {
                return tempPiece;
            }
        }
        return null;
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