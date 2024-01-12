using UnityEngine;

internal class CustomRulesStrategy : RulesStrategy
{
    internal override int BoardSize { get; }
    internal override Color PlayableTileColor { get; }
    internal override int RowsPerTeam { get; }
    internal override Color DarkPieceColor { get; }
    internal override GameColor StartingPieceColor { get; }
    internal override bool FlyingKing { get; }
    internal override bool PawnCanCaptureBackwards { get; }

    internal CustomRulesStrategy(int boardSize, Color playableTileColor, int rowsPerTeam, Color darkPieceColor, GameColor startingPieceColor, bool flyingKing, bool pawnCanCaptureBackwards)
    {
        BoardSize = boardSize;
        PlayableTileColor = playableTileColor;
        RowsPerTeam = rowsPerTeam;
        DarkPieceColor = darkPieceColor;
        StartingPieceColor = startingPieceColor;
        FlyingKing = flyingKing;
        PawnCanCaptureBackwards = pawnCanCaptureBackwards;
    }
}