using UnityEngine;

internal class American : Rules
{
    internal override int BoardSize => 8;
    internal override Color PlayableTileColor => Color.green;
    internal override int RowsPerTeam => 3;
    internal override Color DarkPieceColor => Color.red;
}