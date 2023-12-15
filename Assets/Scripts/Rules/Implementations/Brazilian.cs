using UnityEngine;

internal class Brazilian : Rules
{
    internal override int BoardSize => 8;
    internal override Color PlayableTileColor => Color.black;
    internal override int RowsPerTeam => 3;
    internal override Color DarkPieceColor => Color.black;
}