using UnityEngine;

internal class BrazilianStrategy : RulesStrategy
{
    internal override int BoardSize => 8;
    internal override Color PlayableTileColor => Color.black;
    internal override int RowsPerTeam => 3;
    internal override Color DarkPieceColor => Color.black;
    internal override GameColor StartingPieceColor => GameColor.Light;
}