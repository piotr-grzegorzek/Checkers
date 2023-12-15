using UnityEngine;

internal class International : Rules
{
    internal override int BoardSize => 10;
    internal override Color PlayableTileColor => Color.black;
    internal override int RowsPerTeam => 4;
    internal override Color DarkPieceColor => Color.black;
}