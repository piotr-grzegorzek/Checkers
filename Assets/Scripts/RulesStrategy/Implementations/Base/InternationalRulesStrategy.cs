using UnityEngine;

internal class InternationalRulesStrategy : RulesStrategy
{
    internal override int BoardSize => 10;
    internal override Color PlayableTileColor => Color.black;
    internal override int RowsPerTeam => 4;
    internal override Color DarkPieceColor => Color.black;
    internal override GameColor StartingPieceColor => GameColor.Light;
    internal override bool FlyingKing => throw new System.NotImplementedException();
    internal override bool PawnCanCaptureBackwards => throw new System.NotImplementedException();
}