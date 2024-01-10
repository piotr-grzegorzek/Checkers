using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class Utils : MonoBehaviour
{
    internal static IEnumerable<Piece> GetPiecesOfColor(GameColor color)
    {
        return FindObjectsOfType<Piece>().Where(p => p.PieceColor == color);
    }
}