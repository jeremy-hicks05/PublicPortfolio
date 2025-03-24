namespace ConsoleChessV2.Pieces
{
    internal class WhiteRook : Piece
    {
        public WhiteRook()
        {
            Name = "[R]";
            PointValue = 5;
            BelongsTo = Player.White;
        }

        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            if(fromSpace == toSpace)
            {
                return false;
            }
            if (fromSpace.Column == toSpace.Column || fromSpace.Row == toSpace.Row)
            {
                return true;
            }
            return false;
        }
        public override void CreateListOfPiecesToInspect(Space fromSpace, Space toSpace)
        {
            spacesToMoveToReview?.Clear();
            if (toSpace.Column > fromSpace.Column && toSpace.Row == fromSpace.Row)
            {
                // attacking right
                for (int column = fromSpace.Column + 1; column <= toSpace.Column; column++)
                {
                    spacesToMoveToReview!.Add(ChessBoard.Spaces![column][fromSpace.Row]);
                }
            }
            else if (toSpace.Column < fromSpace.Column && toSpace.Row == fromSpace.Row)
            {
                // attacking left
                for (int column = fromSpace.Column - 1; column >= toSpace.Column; column--)
                {
                    spacesToMoveToReview!.Add(ChessBoard.Spaces![column][fromSpace.Row]);
                }
            }
            else if (toSpace.Column == fromSpace.Column && toSpace.Row < fromSpace.Row)
            {
                // attacking down
                for (int row = fromSpace.Row - 1; row >= toSpace.Row; row--)
                {
                    spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column][row]);
                }
            }
            else if (toSpace.Column == fromSpace.Column && toSpace.Row > fromSpace.Row)
            {
                // attacking up
                for (int row = fromSpace.Row + 1; row <= toSpace.Row; row++)
                {
                    spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column][row]);
                }
            }
        }
    }
}
