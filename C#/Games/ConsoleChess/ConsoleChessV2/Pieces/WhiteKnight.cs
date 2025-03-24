namespace ConsoleChessV2.Pieces
{
    internal class WhiteKnight : Piece
    {
        public WhiteKnight()
        {
            Name = "[N]";
            PointValue = 1;
            BelongsTo = Player.White;
        }
        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
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
            //if(fromSpace.Column + 1 <= 7 && fromSpace.Row + 2 <= 7)
            //{
            //    spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column + 1][fromSpace.Row + 2]);
            //}
            //
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
