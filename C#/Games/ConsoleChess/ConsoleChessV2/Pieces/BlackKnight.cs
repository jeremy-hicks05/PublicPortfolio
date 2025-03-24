namespace ConsoleChessV2.Pieces
{
    internal class BlackKnight : Piece
    {
        public BlackKnight()
        {
            Name = "[n]";
            PointValue = 3;
            BelongsTo = Player.Black;
        }
        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            if (fromSpace == toSpace)
            {
                return false;
            }
            if ((Math.Abs(fromSpace.Column - toSpace.Column) == 1 && Math.Abs(fromSpace.Row - toSpace.Row) == 2) ||
                (Math.Abs(fromSpace.Column - toSpace.Column) == 2 && Math.Abs(fromSpace.Row - toSpace.Row) == 1))
            {
                return true;
            }
            return false;
        }
        public override void CreateListOfPiecesToInspect(Space fromSpace, Space toSpace)
        {
            spacesToMoveToReview?.Clear();
            spacesToMoveToReview!.Add(toSpace);

            //solve out of bounds issues for knights
            //spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column + 1][fromSpace.Row + 2]);
            //spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column + 1][fromSpace.Row - 2]);
            //spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column - 1][fromSpace.Row + 2]);
            //spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column - 1][fromSpace.Row - 2]);
            //spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column + 2][fromSpace.Row + 1]);
            //spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column + 2][fromSpace.Row - 1]);
            //spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column - 2][fromSpace.Row + 1]);
            //spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column - 2][fromSpace.Row - 1]);
        }
    }
}
