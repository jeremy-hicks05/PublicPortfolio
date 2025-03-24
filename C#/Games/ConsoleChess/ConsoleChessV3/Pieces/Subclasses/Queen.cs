using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChessV3.Pieces.Subclasses
{
    internal class Queen : Piece
    {
        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return fromSpace.Column == toSpace.Column ||
                fromSpace.Row == toSpace.Row ||
                Math.Abs(fromSpace.Column - toSpace.Column) /
                (float)Math.Abs(fromSpace.Row - toSpace.Row) == 1; // && shortcircuit prevents div by 0
        }

        public override void BuildListOfSpacesToInspect(Space fromSpace, Space toSpace)
        {
            // Move like Rook
            SpacesToReview.Clear();
            if (toSpace.Column > fromSpace.Column && toSpace.Row == fromSpace.Row)
            {
                // attacking right
                for (int column = fromSpace.Column + 1; column <= toSpace.Column; column++)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(column, fromSpace.Row));
                }
            }
            else if (toSpace.Column < fromSpace.Column && toSpace.Row == fromSpace.Row)
            {
                // attacking left
                for (int column = fromSpace.Column - 1; column >= toSpace.Column; column--)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(column, fromSpace.Row));
                }
            }
            else if (toSpace.Column == fromSpace.Column && toSpace.Row < fromSpace.Row)
            {
                // attacking down
                for (int row = fromSpace.Row - 1; row >= toSpace.Row; row--)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column, row));
                }
            }
            else if (toSpace.Column == fromSpace.Column && toSpace.Row > fromSpace.Row)
            {
                // attacking up
                for (int row = fromSpace.Row + 1; row <= toSpace.Row; row++)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column, row));
                }
            }
            // end Move like Rook
            // Move like Bishop
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
            // end Move like Bishop
        }
    }
}
