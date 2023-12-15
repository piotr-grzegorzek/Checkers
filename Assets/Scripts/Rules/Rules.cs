using UnityEngine;

internal abstract class Rules
{
    internal abstract int BoardSize { get; }
    internal abstract Color PlayableTileColor { get; }
    internal abstract int RowsPerTeam { get; }
    internal abstract Color DarkPieceColor { get; }
}