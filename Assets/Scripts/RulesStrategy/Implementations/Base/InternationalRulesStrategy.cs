using UnityEngine;

internal class InternationalRulesStrategy : RulesStrategy
{
    internal override int BoardSize => 10;
    internal override Color PlayableTileColor => Color.black;
    internal override int RowsPerTeam => 4;
    internal override Color DarkPieceColor => new Color(0.5f, 0.5f, 0.5f);
    internal override GameColor StartingPieceColor => GameColor.Light;
    internal override bool FlyingKing => true;
    internal override bool PawnCanCaptureBackwards => true;
}