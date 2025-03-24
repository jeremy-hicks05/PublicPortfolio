namespace ConsoleChessV2.Pieces
{
    internal class WhiteQueen : Piece
    {
        public WhiteQueen()
        {
            Name = "[Q]";
            PointValue = 9;
            BelongsTo = Player.White;
        }
        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            if (fromSpace == toSpace)
            {
                return false;
            }
            // move like rook
            if (fromSpace.Column == toSpace.Column || fromSpace.Row == toSpace.Row)
            {
                return true;
            }

            // move like bishop
            if (fromSpace.Column != toSpace.Column && fromSpace.Row != toSpace.Row)
            {
                if ((float)Math.Abs(fromSpace.Column - toSpace.Column) / (float)Math.Abs(fromSpace.Row - toSpace.Row) == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public override void CreateListOfPiecesToInspect(Space fromSpace, Space toSpace)
        {
            spacesToMoveToReview?.Clear();
            // move like rook
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
            // move like bishop
            if (toSpace.Column > fromSpace.Column && toSpace.Row > fromSpace.Row)
            {
                // attacking up and right
                for (int column = fromSpace.Column + 1, row = fromSpace.Row + 1; column <= toSpace.Column && row <= toSpace.Row; column++, row++)
                {
                    spacesToMoveToReview!.Add(ChessBoard.Spaces![column][row]);
                }
            }
            else if (toSpace.Column > fromSpace.Column && toSpace.Row < fromSpace.Row)
            {
                // attacking down and right
                for (int column = fromSpace.Column + 1, row = fromSpace.Row - 1; column <= toSpace.Column && row >= toSpace.Row; column++, row--)
                {
                    spacesToMoveToReview!.Add(ChessBoard.Spaces![column][row]);
                }
            }
            else if (toSpace.Column < fromSpace.Column && toSpace.Row < fromSpace.Row)
            {
                // attacking down and left
                for (int column = fromSpace.Column - 1, row = fromSpace.Row - 1; column >= toSpace.Column && row >= toSpace.Row; column--, row--)
                {
                    spacesToMoveToReview!.Add(ChessBoard.Spaces![column][row]);
                }
            }
            else if (toSpace.Column < fromSpace.Column && toSpace.Row > fromSpace.Row)
            {
                // attacking up and left
                for (int column = fromSpace.Column - 1, row = fromSpace.Row + 1; column >= toSpace.Column && row <= toSpace.Row; column--, row++)
                {
                    spacesToMoveToReview!.Add(ChessBoard.Spaces![column][row]);
                }
            }
        }


    }
}
