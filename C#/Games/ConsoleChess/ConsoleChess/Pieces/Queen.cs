using ConsoleChess.Interfaces;
using ConsoleChess.Enums;

namespace ConsoleChess.Pieces
{
    internal class Queen : Piece
    {
        public Queen(string name, Player belongsTo) : base(name, belongsTo)
        {

        }

        public override bool CanTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            // test whether attempted move follows the rules of chess
            if (fromSpace == toSpace)
            {
                return false;
            }
            // if move like bishop
            if ((float)(Math.Abs(fromSpace.Letter - toSpace.Letter)) / (float)(Math.Abs(fromSpace.Number - toSpace.Number)) == 1)
            {
                return true;
            }
            // if move like rook
            if (fromSpace.Letter == toSpace.Letter || fromSpace.Number == toSpace.Number)
            {
                return true;
            }
            // fallout false
            return false;
        }

        public override bool HasPiecesBlockingMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            // MOVE AS BISHOP
            // if toSpace is down and right
            if (fromSpace.Letter < toSpace.Letter && fromSpace.Number < toSpace.Number)
            {
                for (int i = fromSpace.Letter + 1, k = fromSpace.Number + 1; i <= toSpace.Letter && k <= toSpace.Number; i++, k++)
                {
                    if (i == toSpace.Letter && k == toSpace.Number)
                    {
                        // is not blocked
                        return false;
                    }
                    if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
                    {
                        // is blocked
                        return true;
                    }
                }
            }
            // if toSpace is down and left
            else if (fromSpace.Letter < toSpace.Letter && fromSpace.Number > toSpace.Number)
            {
                for (int i = fromSpace.Letter + 1, k = fromSpace.Number - 1; i <= toSpace.Letter && k >= toSpace.Number; i++, k--)
                {
                    if ((i == toSpace.Letter && k == toSpace.Number))
                    {
                        // is not blocked
                        return false;
                    }
                    if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
                    {
                        // is blocked
                        return true;
                    }
                }
            }
            // if toSpace is up and right
            else if (fromSpace.Letter > toSpace.Letter && fromSpace.Number < toSpace.Number)
            {
                for (int i = fromSpace.Letter - 1, k = fromSpace.Number + 1; i >= toSpace.Letter && k <= toSpace.Number; i--, k++)
                {
                    if ((i == toSpace.Letter && k == toSpace.Number))
                    {
                        // is not blocked
                        return false;
                    }
                    if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
                    {
                        // is blocked
                        return true;
                    }
                }
            }
            // if toSpace is up and left
            else if (fromSpace.Letter > toSpace.Letter && fromSpace.Number > toSpace.Number)
            {
                for (int i = fromSpace.Letter - 1, k = fromSpace.Number - 1; i >= toSpace.Letter && k >= toSpace.Number; i--, k--)
                {
                    if ((i == toSpace.Letter && k == toSpace.Number))
                    {
                        // is not blocked
                        return false;
                    }
                    if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
                    {
                        // is blocked
                        return true;
                    }
                }
            }
            // MOVE AS ROOK
            // if space is above
            if (fromSpace.Letter > toSpace.Letter)
            {
                for (int i = fromSpace.Letter - 1; i >= toSpace.Letter; i--)
                {
                    if (i == toSpace.Letter)
                    {
                        // not blocked
                        return false;
                    }

                    if (Board.spaces[i][toSpace.Number].Piece.belongsToPlayer !=
                        Player.None)
                    {
                        // blocked
                        return true;
                    }
                }
            }
            else if (fromSpace.Letter < toSpace.Letter)
            {
                // if space is below
                for (int i = fromSpace.Letter + 1; i <= toSpace.Letter; i++)
                {
                    if (i == toSpace.Letter)
                    {
                        // not blocked
                        return false;
                    }

                    if (Board.spaces[i][toSpace.Number].Piece.belongsToPlayer !=
                        Player.None)
                    {
                        // blocked
                        return true;
                    }
                }
            }
            // if space is to the left
            else if (fromSpace.Number > toSpace.Number)
            {
                for (int i = fromSpace.Number - 1; i >= toSpace.Number; i--)
                {
                    if (i == toSpace.Number)
                    {
                        // not blocked
                        return false;
                    }

                    if (Board.spaces[toSpace.Letter][i].Piece.belongsToPlayer !=
                        Player.None)
                    {
                        // blocked
                        return true;
                    }
                }
            }
            else if (fromSpace.Number < toSpace.Number)
            {
                // if space is to the right
                for (int i = fromSpace.Number + 1; i <= toSpace.Number; i++)
                {
                    if (i == toSpace.Number)
                    {
                        // not blocked
                        return false;
                    }

                    if (Board.spaces[toSpace.Letter][i].Piece.belongsToPlayer !=
                        Player.None)
                    {
                        // blocked
                        return true;
                    }
                }
            }
            // fallout true
            return true;
        }

        public override bool CanTryToCapture(Space fromSpace, Space toSpace)
        {
            return false;
            //if (fromSpace == toSpace)
            //{
            //    return false;
            //}
            //// if MOVE LIKE A ROOK
            //if ((fromSpace.Letter == toSpace.Letter) || (fromSpace.Number == toSpace.Number))
            //{
            //    // if space is to the right -> above?
            //    if (fromSpace.Letter > toSpace.Letter && fromSpace.Number == toSpace.Number)
            //    {
            //        for (int i = fromSpace.Letter - 1; i >= toSpace.Letter; i--)
            //        {
            //            if (i == toSpace.Letter)
            //            {
            //                // capture or move to empty space
            //                return true;
            //            }
            //            if (Board.spaces[i][toSpace.Number].Piece.belongsToPlayer != Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    else if (fromSpace.Letter < toSpace.Letter && fromSpace.Number == toSpace.Number)
            //    {
            //        // if space is to the left -> below?
            //        for (int i = fromSpace.Letter + 1; i <= toSpace.Letter; i++)
            //        {
            //            if (i == toSpace.Letter)
            //            {
            //                // capture or move to empty space
            //                return true;
            //            }
            //            if (Board.spaces[i][toSpace.Number].Piece.belongsToPlayer != Player.None)
            //            {
            //                return false;
            //            }
            //        }


            //    }
            //    // if space is above -> to the left?
            //    else if (fromSpace.Number > toSpace.Number && fromSpace.Letter == toSpace.Letter)
            //    {
            //        for (int i = fromSpace.Number - 1; i >= toSpace.Number; i--)
            //        {
            //            if (i == toSpace.Number)
            //            {
            //                // capture or move to empty space
            //                return true;
            //            }
            //            if (Board.spaces[toSpace.Letter][i].Piece.belongsToPlayer != Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    else if (fromSpace.Number < toSpace.Number && fromSpace.Letter == toSpace.Letter)
            //    {
            //        // if space is below -> to the right??
            //        for (int i = fromSpace.Number + 1; i <= toSpace.Number; i++)
            //        {
            //            if (i == toSpace.Number)
            //            {
            //                // capture or move to empty space
            //                return true;
            //            }
            //            if (Board.spaces[toSpace.Letter][i].Piece.belongsToPlayer != Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //} // end MOVE LIKE A ROOK

            //// MOVE LIKE A BISHOP
            //if ((float)(Math.Abs(fromSpace.Letter - toSpace.Letter)) / (float)(Math.Abs(fromSpace.Number - toSpace.Number)) == 1)
            //{
            //    // from toSpace up to fromSpace - if any spaces contain pieces, return false
            //    // if toSpace is down and right
            //    if (fromSpace.Letter < toSpace.Letter && fromSpace.Number < toSpace.Number)
            //    {
            //        for (int i = fromSpace.Letter + 1, k = fromSpace.Number + 1; i <= toSpace.Letter && k <= toSpace.Number; i++, k++)
            //        {
            //            if ((i == toSpace.Letter || k == toSpace.Number))
            //            {
            //                // capture or move to empty space
            //                return true;
            //            }
            //            if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    // if toSpace is down and left
            //    else if (fromSpace.Letter < toSpace.Letter && fromSpace.Number > toSpace.Number)
            //    {
            //        for (int i = fromSpace.Letter + 1, k = fromSpace.Number - 1; i <= toSpace.Letter && k >= toSpace.Number; i++, k--)
            //        {
            //            if ((i == toSpace.Letter || k == toSpace.Number))
            //            {
            //                // capture or move to empty space
            //                return true;
            //            }
            //            if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    // if toSpace is up and right
            //    else if (fromSpace.Letter > toSpace.Letter && fromSpace.Number < toSpace.Number)
            //    {
            //        for (int i = fromSpace.Letter - 1, k = fromSpace.Number + 1; i >= toSpace.Letter && k <= toSpace.Number; i--, k++)
            //        {
            //            if ((i == toSpace.Letter || k == toSpace.Number))
            //            {
            //                // capture or move to empty space
            //                return true;
            //            }
            //            if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    // if toSpace is up and left
            //    else if (fromSpace.Letter > toSpace.Letter && fromSpace.Number > toSpace.Number)
            //    {
            //        for (int i = fromSpace.Letter - 1, k = fromSpace.Number - 1; i >= toSpace.Letter && k >= toSpace.Number; i--, k--)
            //        {
            //            if ((i == toSpace.Letter || k == toSpace.Number))
            //            {
            //                // capture or move to empty space
            //                return true;
            //            }
            //            if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    // success
            //    return true;
            //}
            //// end MOVE LIKE A BISHOP

            //// fallout condition
            //return false;
        }

        public override bool CanMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return fromSpace.Piece.belongsToPlayer != 
                toSpace.Piece.belongsToPlayer &&
                CanTryToCapture(fromSpace, toSpace);
        }
    }
}
