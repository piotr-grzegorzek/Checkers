using UnityEngine;

internal class CustomRulesStrategyFactory
{
    internal static RulesStrategy Create(int boardSize, Color playableTileColor, int rowsPerTeam, Color darkPieceColor, GameColor startingPieceColor, bool flyingKing)
    {
        return new CustomRulesStrategy(boardSize, playableTileColor, rowsPerTeam, darkPieceColor, startingPieceColor, flyingKing);
    }
}