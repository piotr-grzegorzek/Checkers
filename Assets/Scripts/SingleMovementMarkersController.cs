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

    private readonly float _singleTileDistance = Mathf.Sqrt(2);
    private readonly float _jumpDistance = 2 * Mathf.Sqrt(2);
    private readonly float _pieceCheckRadius = 0.1f;

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
        IEnumerable<Piece> currentPlayerPieces = Utils.GetPiecesOfColor(piece.PieceColor);
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
        piece.MoveTo(marker.transform.position);

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
        bool isOccupied = GetPieceFromCollider(Physics.OverlapSphere(tile.transform.position, _pieceCheckRadius)) != null;
        if (isOccupied)
        {
            return false;
        }

        // Get the distance between the piece and the tile
        float distance = Vector3.Distance(
            new Vector3(piece.transform.position.x, 0, piece.transform.position.z),
            new Vector3(tile.transform.position.x, 0, tile.transform.position.z));

        // Handle piece's movement based on its type
        return piece.Type switch
        {
            PieceType.King => HandleKingMovement(tile, piece, darkTiles, capturablePieces, distance),
            _ => HandlePawnMovement(tile, piece, darkTiles, capturablePieces, distance),
        };
    }
    private void InstantiateMovementMarker(Tile tile, Piece piece, List<Piece> capturablePieces)
    {
        Vector3 offset = new Vector3(0, Utils.PieceUpOffset, 0);
        Vector3 newMarkerPosition = tile.transform.position + offset;
        GameObject marker = Instantiate(_movementMarkerPrefab, newMarkerPosition, Quaternion.identity, _movementMarkersGameObject.transform);
        MovementMarker markerScript = marker.GetComponent<MovementMarker>();
        markerScript.SourcePiece = piece;
        markerScript.CapturablePieces = capturablePieces;
    }
    private bool HandleKingMovement(Tile tile, Piece piece, IEnumerable<Tile> darkTiles, List<Piece> capturablePieces, float distance)
    {
        // If rules allow flying king, it can move any amount of tiles
        if (SingleRulesContext.Instance.Rules.FlyingKing)
        {
            // Implement the logic for flying king
            // Check if the movement is diagonal
            if (Mathf.Abs(piece.transform.position.x - tile.transform.position.x) != Mathf.Abs(piece.transform.position.z - tile.transform.position.z))
            {
                // The movement is not diagonal
                return false;
            }

            Vector3 direction = (tile.transform.position - piece.transform.position).normalized;
            float distanceToTile = Vector3.Distance(piece.transform.position, tile.transform.position);
            RaycastHit[] hits = Physics.RaycastAll(piece.transform.position, direction, distanceToTile);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.TryGetComponent<Piece>(out var hitPiece))
                {
                    // If there's a piece between the king and the target tile, the king can capture it
                    if (hitPiece.PieceColor != piece.PieceColor)
                    {
                        capturablePieces.Add(hitPiece);
                    }
                    else
                    {
                        // If there's a piece of the same color, the king cannot move
                        return false;
                    }
                }
            }
            // If there are no pieces of the same color between the king and the target tile, the king can move
            return true;
        }
        else
        {
            // If not a flying king, it can move only 1 tile or capture
            if (distance == _singleTileDistance)
            {
                // Single tile movement
                return true;
            }
            else
            {
                // If not a flying king, it can move only 1 tile or capture
                if (distance == _singleTileDistance)
                {
                    // Single tile movement
                    return true;
                }
                else if (distance == _jumpDistance)
                {
                    // Jumping over a piece
                    return HandleJumpOverPiece(tile, piece, darkTiles, capturablePieces);
                }
            }
        }

        return false;
    }
    private bool HandlePawnMovement(Tile tile, Piece piece, IEnumerable<Tile> darkTiles, List<Piece> capturablePieces, float distance)
    {
        if (distance == _singleTileDistance)
        {
            // Single tile movement
            // Check if the pawn is moving backwards
            if ((piece.PieceColor == GameColor.Light && tile.transform.position.z < piece.transform.position.z) ||
                (piece.PieceColor == GameColor.Dark && tile.transform.position.z > piece.transform.position.z))
            {
                // The pawn is moving backwards
                return false;
            }
            return true;
        }
        else if (distance == _jumpDistance)
        {
            // Jumping over a piece
            return HandleJumpOverPiece(tile, piece, darkTiles, capturablePieces);
        }

        return false;
    }
    private bool HandleJumpOverPiece(Tile tile, Piece piece, IEnumerable<Tile> darkTiles, List<Piece> capturablePieces)
    {
        // Get the middle tile
        Vector3 middlePoint = Vector3.Lerp(piece.transform.position, tile.transform.position, 0.5f);
        Tile middleTile = darkTiles.FirstOrDefault(t =>
            (int)t.transform.position.x == (int)middlePoint.x &&
            (int)t.transform.position.z == (int)middlePoint.z);
        if (middleTile != null)
        {
            // Check if the middle tile is occupied by an enemy piece
            Piece middlePiece = GetPieceFromCollider(Physics.OverlapSphere(middleTile.transform.position, _pieceCheckRadius));
            if (middlePiece != null && middlePiece.PieceColor != piece.PieceColor)
            {
                // Add the capturable piece to the list
                capturablePieces.Add(middlePiece);
                return true;
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
}