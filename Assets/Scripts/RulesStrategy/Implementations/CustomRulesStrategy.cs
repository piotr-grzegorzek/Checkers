using UnityEngine;

internal class CustomRulesStrategy : RulesStrategy
{
    internal override int BoardSize { get; }
    internal override Color PlayableTileColor { get; }
    internal override int RowsPerTeam { get; }
    internal override Color DarkPieceColor { get; }
    internal override GameColor StartingPieceColor { get; }

    internal CustomRulesStrategy(int boardSize, Color playableTileColor, int rowsPerTeam, Color darkPieceColor, GameColor startingPieceColor)
    {
        BoardSize = boardSize;
        PlayableTileColor = playableTileColor;
        RowsPerTeam = rowsPerTeam;
        DarkPieceColor = darkPieceColor;
        StartingPieceColor = startingPieceColor;
    }
}