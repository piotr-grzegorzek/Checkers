using UnityEngine;

public class Brazilian : Rules
{
    public override int BoardSize => 8;
    public override Color PlayableTileColor => Color.black;
    public override int RowsPerTeam => 3;
    public override Color DarkPieceColor => Color.black;
}