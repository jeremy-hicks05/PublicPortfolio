using ConsoleChessV3.Interfaces;

namespace ConsoleChessV3.Pieces.Subclasses
{
    internal class Bishop : Piece
    {
        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            if (fromSpace.Column != toSpace.Column && fromSpace.Row != toSpace.Row) // prevent dividing by zero
            {
                if (Math.Abs(fromSpace.Column - toSpace.Column) / (float)Math.Abs(fromSpace.Row - toSpace.Row) == 1)
                {
                    // slope meets criteria
                    return true;
                }
            }
            // slope does not meet criteria
            return false;
        }

        public override void BuildListOfSpacesToInspect(Space fromSpace, Space toSpace)
        {
            SpacesToReview.Clear();

            if (toSpace.Column > fromSpace.Column && toSpace.Row > fromSpace.Row)
            {
                // attacking up and right
                for (int column = fromSpace.Column + 1, row = fromSpace.Row + 1; column <= toSpace.Column && row <= toSpace.Row; column++, row++)
                {
                    SpacesToReview!.Add(ChessBoard.GetSpace(column, row));
                }
            }
            else if (toSpace.Column > fromSpace.Column && toSpace.Row < fromSpace.Row)
            {
                // attacking down and right
                for (int column = fromSpace.Column + 1, row = fromSpace.Row - 1; column <= toSpace.Column && row >= toSpace.Row; column++, row--)
                {
                    SpacesToReview!.Add(ChessBoard.GetSpace(column, row));
                }
            }
            else if (toSpace.Column < fromSpace.Column && toSpace.Row < fromSpace.Row)
            {
                // attacking down and left
                for (int column = fromSpace.Column - 1, row = fromSpace.Row - 1; column >= toSpace.Column && row >= toSpace.Row; column--, row--)
                {
                    SpacesToReview!.Add(ChessBoard.GetSpace(column, row));
                }
            }
            else if (toSpace.Column < fromSpace.Column && toSpace.Row > fromSpace.Row)
            {
                // attacking up and left
                for (int column = fromSpace.Column - 1, row = fromSpace.Row + 1; column >= toSpace.Column && row <= toSpace.Row; column--, row++)
                {
                    SpacesToReview!.Add(ChessBoard.GetSpace(column, row));
                }
            }
        }
    }
}
