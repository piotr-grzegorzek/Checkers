using UnityEngine;

internal class AmericanRulesStrategy : RulesStrategy
{
    internal override int BoardSize => 8;
    internal override Color PlayableTileColor => new(0.47f, 0.68f, 0.51f);
    internal override int RowsPerTeam => 3;
    internal override Color DarkPieceColor => new(0.73f, 0.25f, 0.25f);
    internal override GameColor StartingPieceColor => GameColor.Dark;
    internal override bool FlyingKing => false;
    internal override bool PawnCanCaptureBackwards => false;
}