using UnityEngine;

internal class AmericanRulesStrategy : RulesStrategy
{
    internal override int BoardSize => 8;
    internal override Color PlayableTileColor => Color.green;
    internal override int RowsPerTeam => 3;
    internal override Color DarkPieceColor => Color.red;
    internal override GameColor StartingPieceColor => GameColor.Dark;
    internal override bool FlyingKing => throw new System.NotImplementedException();
}