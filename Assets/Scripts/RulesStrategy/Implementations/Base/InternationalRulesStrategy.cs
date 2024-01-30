using UnityEngine;

internal class InternationalRulesStrategy : RulesStrategy
{
    internal override int BoardSize => 10;
    internal override Color PlayableTileColor => new(0.63f, 0.59f, 0.57f);

    internal override int RowsPerTeam => 4;
    internal override Color DarkPieceColor => new(0.4f, 0.29f, 0.27f);
    internal override GameColor StartingPieceColor => GameColor.Light;
    internal override bool FlyingKing => true;
    internal override bool PawnCanCaptureBackwards => true;
}