using System.Collections.Generic;
using UnityEngine;

public class MovementMarker : MonoBehaviour
{
    internal Piece SourcePiece { get; set; }
    internal List<Piece> CapturablePieces { get; set; }
}