using UnityEngine;

internal class CustomRulesStrategyFactory
{
    internal static RulesStrategy Create(int boardSize, Color playableTileColor, int rowsPerTeam, Color darkPieceColor, GameColor startingPieceColor)
    {
        return new CustomRulesStrategy(boardSize, playableTileColor, rowsPerTeam, darkPieceColor, startingPieceColor);
    }
}