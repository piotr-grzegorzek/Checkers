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
        Tile[] tiles = FindObjectsOfType<Tile>();
        IEnumerable<Tile> filtered = tiles.Where(tile =>
        {
            // Calculate the difference in row and column between the piece and the tile
            int rowDiff = (int)Mathf.Abs(tile.transform.position.x - piece.transform.position.x);
            int colDiff = (int)Mathf.Abs(tile.transform.position.z - piece.transform.position.z);

            // Check if the tile is on the diagonal from the piece
            return rowDiff == colDiff;
        });

        Vector3 offset = new Vector3(0, 0.5f, 0);
        foreach (var tile in filtered)
        {
            Vector3 newMarkerPosition = tile.transform.position + offset;
            GameObject marker = Instantiate(_movementMarkerPrefab, newMarkerPosition, Quaternion.identity, _movementMarkersGameObject.transform);
            MovementMarker markerScript = marker.GetComponent<MovementMarker>();
            markerScript.SourcePiece = piece;
            markerScript.CapturablePieces = new List<Piece>();
        }
    }

    internal void ClearMovementMarkers()
    {
        foreach (var marker in FindObjectsOfType<MovementMarker>())
        {
            Destroy(marker.gameObject);
        }
    }
}