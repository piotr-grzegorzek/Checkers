using UnityEngine;

public class American : Rules
{
    public override int BoardSize => 8;
    public override Color PlayableTileColor => Color.green;
    public override int RowsPerTeam => 3;
    public override Color DarkPieceColor => Color.red;
}