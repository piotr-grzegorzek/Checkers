using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class Utils : MonoBehaviour
{
    internal static float PieceUpOffset = 0.5f;

    internal static IEnumerable<Piece> GetPiecesOfColor(GameColor color)
    {
        return FindObjectsOfType<Piece>().Where(p => p.PieceColor == color);
    }
}