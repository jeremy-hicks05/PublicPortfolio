using ConsoleChess.Interfaces;
using ConsoleChess.Enums;

namespace ConsoleChess.Pieces
{
    internal class Rook : Piece
    {
        public Rook(string name, Player belongsTo) : base(name, belongsTo)
        {
            hasMoved = false;
        }

        public override bool CanTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            if (fromSpace == toSpace)
            {
                return false;
            }
            else if (fromSpace.Letter == toSpace.Letter || fromSpace.Number == toSpace.Number)
            {
                return true;
            }
            // fallout false
            return false;
        }

        public override bool HasPiecesBlockingMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
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
            //else if (fromSpace.Letter == toSpace.Letter || fromSpace.Number == toSpace.Number)
            //{
            //    // if space is above
            //    if (fromSpace.Letter > toSpace.Letter)
            //    {
            //        for (int i = fromSpace.Letter - 1; i >= toSpace.Letter; i--)
            //        {
            //            if (i == toSpace.Letter)
            //            {
            //                return true;
            //            }

            //            if (Board.spaces[i][toSpace.Number].Piece.belongsToPlayer != 
            //                Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    else if (fromSpace.Letter < toSpace.Letter)
            //    {
            //        // if space is below
            //        for (int i = fromSpace.Letter + 1; i <= toSpace.Letter; i++)
            //        {
            //            if (i == toSpace.Letter)
            //            {
            //                return true;
            //            }

            //            if (Board.spaces[i][toSpace.Number].Piece.belongsToPlayer != 
            //                Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    // if space is to the left
            //    else if (fromSpace.Number > toSpace.Number)
            //    {
            //        for (int i = fromSpace.Number - 1; i >= toSpace.Number; i--)
            //        {
            //            if (i == toSpace.Number)
            //            {
            //                return true;
            //            }

            //            if (Board.spaces[toSpace.Letter][i].Piece.belongsToPlayer != 
            //                Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    else if (fromSpace.Number < toSpace.Number)
            //    {
            //        // if space is to the right
            //        for (int i = fromSpace.Number + 1; i <= toSpace.Number; i++)
            //        {
            //            if (i == toSpace.Number)
            //            {
            //                return true;
            //            }

            //            if (Board.spaces[toSpace.Letter][i].Piece.belongsToPlayer != 
            //                Player.None)
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //}

            //// fallout condition
            //return false;
        }

        public override bool CanMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return fromSpace.Piece.belongsToPlayer != 
                toSpace.Piece.belongsToPlayer
                && CanTryToCapture(fromSpace, toSpace);
        }
    }
}
