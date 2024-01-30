using UnityEngine;

internal class BrazilianRulesStrategy : RulesStrategy
{
    internal override int BoardSize => 8;
    internal override Color PlayableTileColor => Color.black;
    internal override int RowsPerTeam => 3;
    internal override Color DarkPieceColor => new Color(0.5f, 0.5f, 0.5f);
    internal override GameColor StartingPieceColor => GameColor.Light;
    internal override bool FlyingKing => true;
    internal override bool PawnCanCaptureBackwards => true;
}