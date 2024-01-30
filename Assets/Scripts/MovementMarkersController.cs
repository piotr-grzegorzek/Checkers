using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementMarkersController : MonoBehaviour
{
    [SerializeField]
    GameObject _movementMarkerPrefab;
    [SerializeField]
    GameObject _movementMarkersGameObject;
    [SerializeField]
    Board _board;


    private RulesContext _rulesContext;

    void Start()
    {
        _rulesContext = FindObjectOfType<RulesContext>();
    }

    internal void MakeMovementMarkers(Piece piece)
    {
        ClearMovementMarkers();

        // Get pieces from the current player
        IEnumerable<Piece> currentPlayerPieces = _board.GetPiecesOfColor(piece.PieceColor);
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
                MovementMarker newMarker = InstantiateMovementMarker(preparedMovementMarker.Tile, preparedMovementMarker.Piece, preparedMovementMarker.CapturablePieces, null);
                InstantiateMultiJumps(preparedMovementMarker.Tile, preparedMovementMarker.Piece, darkTiles, newMarker);
            }
        }
        else
        {
            // Instantiate movement markers with Piece = piece only
            foreach (var preparedMovementMarker in preparedMovementMarkers.Where(m => m.Piece == piece))
            {
                InstantiateMovementMarker(preparedMovementMarker.Tile, preparedMovementMarker.Piece, preparedMovementMarker.CapturablePieces, null);
            }
        }
    }
    internal void CommitMovementMarker(MovementMarker marker)
    {
        // Move the piece to the marker
        Piece piece = marker.SourcePiece;
        piece.MoveTo(marker.transform.position);

        if (marker.CapturablePieces.Count == 0 && piece.Type == PieceType.King)
        {
            _board.RegisterKingMoveWithoutCapture(piece.PieceColor);
        }
        // Capture the pieces from marker.CapturablePieces and marker.SourceMarker.CapturablePieces
        foreach (var capturablePiece in marker.CapturablePieces)
        {
            Destroy(capturablePiece.gameObject);
            _board.RegisterPieceCapture();
        }
        if (marker.SourceMarker != null)
        {
            foreach (var capturablePiece in marker.SourceMarker.CapturablePieces)
            {
                Destroy(capturablePiece.gameObject);
                _board.RegisterPieceCapture();
            }
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
    private bool PrepareMovementMarker(Tile tile, Piece piece, IEnumerable<Tile> darkTiles, List<Piece> capturablePieces, Tile multiJumpTile = null)
    {
        // if multiJumpTile is not null, we are doing check for multi-jump, else its normal check
        MonoBehaviour current = (multiJumpTile != null) ? multiJumpTile : piece;

        // Allow only empty tiles
        bool isOccupied = GetPieceFromCollider(Physics.OverlapSphere(tile.transform.position, _board.PieceCheckRadius)) != null;
        if (isOccupied)
        {
            return false;
        }

        // Get the distance between the piece and the tile
        float distance = Vector3.Distance(
            new Vector3(current.transform.position.x, 0, current.transform.position.z),
            new Vector3(tile.transform.position.x, 0, tile.transform.position.z));

        // Handle piece's movement based on its type
        return piece.Type switch
        {
            PieceType.King => HandleKingMovement(tile, piece, darkTiles, capturablePieces, distance, current),
            _ => HandlePawnMovement(tile, piece, darkTiles, capturablePieces, distance, current),
        };
    }
    private bool HandleKingMovement(Tile tile, Piece piece, IEnumerable<Tile> darkTiles, List<Piece> capturablePieces, float distance, MonoBehaviour current)
    {
        // If rules allow flying king, it can move any amount of tiles
        if (_rulesContext.Rules.FlyingKing)
        {
            // Implement the logic for flying king
            // Check if the movement is diagonal
            if (Mathf.Abs(current.transform.position.x - tile.transform.position.x) != Mathf.Abs(current.transform.position.z - tile.transform.position.z))
            {
                // The movement is not diagonal
                return false;
            }

            Vector3 direction = (tile.transform.position - current.transform.position).normalized;
            float distanceToTile = Vector3.Distance(current.transform.position, tile.transform.position);
            RaycastHit[] hits = Physics.RaycastAll(current.transform.position, direction, distanceToTile);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.TryGetComponent<Piece>(out var hitPiece))
                {
                    // If there's a piece between the king and the target tile, the king can capture it
                    if (hitPiece.PieceColor != piece.PieceColor)
                    {
                        // check if on the way to target tile there is atleast one occurence of pieces being 1 tile away from each other
                        // if there is, then king cannot move
                        Vector3 directionToTarget = (tile.transform.position - current.transform.position).normalized;
                        float distanceToTarget = Vector3.Distance(current.transform.position, tile.transform.position);
                        RaycastHit[] hitsToTarget = Physics.RaycastAll(current.transform.position, directionToTarget, distanceToTarget);
                        foreach (RaycastHit hitToTarget in hitsToTarget)
                        {
                            if (hitToTarget.collider.TryGetComponent<Piece>(out var hitPieceToTarget))
                            {
                                if (Vector3.Distance(hitPieceToTarget.transform.position, hitPiece.transform.position) == _board.SingleTileDistance)
                                {
                                    return false;
                                }
                            }
                        }
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
            if (distance == _board.SingleTileDistance)
            {
                // Single tile movement
                return true;
            }
            else
            {
                // If not a flying king, it can move only 1 tile or capture
                if (distance == _board.SingleTileDistance)
                {
                    // Single tile movement
                    return true;
                }
                else if (distance == _board.JumpDistance)
                {
                    // Jumping over a piece
                    return HandleJumpOverPiece(tile, piece, darkTiles, capturablePieces);
                }
            }
        }

        return false;
    }
    private bool HandlePawnMovement(Tile tile, Piece piece, IEnumerable<Tile> darkTiles, List<Piece> capturablePieces, float distance, MonoBehaviour current)
    {
        if (distance == _board.SingleTileDistance)
        {
            // Single tile movement
            // Check if the pawn is moving backwards
            if (IsMovingBackwards(tile, piece, current))
            {
                return false;
            }
            return true;
        }
        else if (distance == _board.JumpDistance)
        {
            if (!_rulesContext.Rules.PawnCanCaptureBackwards)
            {
                // Check if the pawn is moving backwards
                if (IsMovingBackwards(tile, piece, current))
                {
                    return false;
                }
            }
            // Jumping over a piece
            return HandleJumpOverPiece(tile, piece, darkTiles, capturablePieces, current);
        }

        return false;
    }
    private bool IsMovingBackwards(Tile tile, Piece piece, MonoBehaviour current)
    {
        return (piece.PieceColor == GameColor.Light && tile.transform.position.z < current.transform.position.z) ||
            (piece.PieceColor == GameColor.Dark && tile.transform.position.z > current.transform.position.z);
    }
    private bool HandleJumpOverPiece(Tile tile, Piece piece, IEnumerable<Tile> darkTiles, List<Piece> capturablePieces, MonoBehaviour current = null)
    {
        // Get the middle tile
        Vector3 middlePoint = Vector3.Lerp(current.transform.position, tile.transform.position, 0.5f);
        Tile middleTile = darkTiles.FirstOrDefault(t =>
            (int)t.transform.position.x == (int)middlePoint.x &&
            (int)t.transform.position.z == (int)middlePoint.z);
        if (middleTile != null)
        {
            // Check if the middle tile is occupied by an enemy piece
            Piece middlePiece = GetPieceFromCollider(Physics.OverlapSphere(middleTile.transform.position, _board.PieceCheckRadius));
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
    private MovementMarker InstantiateMovementMarker(Tile tile, Piece piece, List<Piece> capturablePieces, MovementMarker sourceMarker)
    {
        Vector3 offset = new Vector3(0, _board.PieceUpOffset, 0);
        Vector3 newMarkerPosition = tile.transform.position + offset;
        GameObject marker = Instantiate(_movementMarkerPrefab, newMarkerPosition, Quaternion.identity, _movementMarkersGameObject.transform);
        MovementMarker markerScript = marker.GetComponent<MovementMarker>();
        markerScript.SourcePiece = piece;
        markerScript.CapturablePieces = capturablePieces;
        markerScript.SourceMarker = sourceMarker;
        return markerScript;
    }
    private void InstantiateMultiJumps(Tile tile, Piece piece, in IEnumerable<Tile> darkTiles, MovementMarker sourceMarker)
    {
        // from dark tiles get only tiles which are _jumpDistance from tile
        IEnumerable<Tile> possibleTiles = darkTiles.Where(t => Vector3.Distance(t.transform.position, tile.transform.position) == _board.JumpDistance);
        foreach (var possibleTile in possibleTiles)
        {
            List<Piece> capturablePieces = new List<Piece>();
            if (PrepareMovementMarker(possibleTile, piece, darkTiles, capturablePieces, tile) && capturablePieces.Count > 0)
            {
                MovementMarker newMarker = InstantiateMovementMarker(possibleTile, piece, capturablePieces, sourceMarker);
                sourceMarker.gameObject.SetActive(false);
                InstantiateMultiJumps(possibleTile, piece, darkTiles, newMarker);
            }
        }
    }
}