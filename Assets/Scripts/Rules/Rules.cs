using UnityEngine;

public abstract class Rules
{
    public abstract int BoardSize { get; }
    public abstract Color PlayableTileColor { get; }
    public abstract int RowsPerTeam { get; }
    public abstract Color DarkPieceColor { get; }
}