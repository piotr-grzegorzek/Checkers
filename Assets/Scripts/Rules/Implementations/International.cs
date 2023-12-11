using UnityEngine;

public class International : Rules
{
    public override int BoardSize => 10;
    public override Color PlayableTileColor => Color.black;
    public override int RowsPerTeam => 4;
    public override Color DarkPieceColor => Color.black;
}