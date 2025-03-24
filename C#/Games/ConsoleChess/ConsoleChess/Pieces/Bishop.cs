using ConsoleChess.Interfaces;
using ConsoleChess.Enums;

namespace ConsoleChess.Pieces
{
    internal class Bishop : Piece
    {
        public Bishop(string name, Player belongsTo) : base(name, belongsTo)
        {

        }

        public override bool CanTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            // test whether attempted move follows the rules of chess
            if (fromSpace == toSpace)
            {
                return false;
            }
            else if (fromSpace.Letter == toSpace.Letter || fromSpace.Number == toSpace.Number)
            {
                return false;
            }
            else if ((float)(Math.Abs(fromSpace.Letter - toSpace.Letter)) / (float)(Math.Abs(fromSpace.Number - toSpace.Number)) == 1)
            {
                return true;
            }
            return false;
        }

        public override bool HasPiecesBlockingMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
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
            // fallout true - blocked
            return true;
        }

        public override bool CanTryToCapture(Space fromSpace, Space toSpace)
        {
            //return false;
            if (fromSpace == toSpace)
            {
                return false;
            }
            if (fromSpace.Letter == toSpace.Letter || fromSpace.Number == toSpace.Number)
            {
                return false;
            }
            if ((float)(Math.Abs(fromSpace.Letter - toSpace.Letter)) / (float)(Math.Abs(fromSpace.Number - toSpace.Number)) == 1)
            {
                // if toSpace is down and right
                if (fromSpace.Letter < toSpace.Letter && fromSpace.Number < toSpace.Number)
                {
                    for (int i = fromSpace.Letter + 1, k = fromSpace.Number + 1; i <= toSpace.Letter && k <= toSpace.Number; i++, k++)
                    {
                        if (i == toSpace.Letter || k == toSpace.Number)
                        {
                            // capture or move to empty space
                            Console.WriteLine("Space:");
                            fromSpace.PrintInfo();
                            Console.WriteLine();
                            Console.WriteLine("Can capture");
                            toSpace.PrintInfo();
                            return true;
                        }
                        if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
                        {
                            return false;
                        }
                    }
                }
                // if toSpace is down and left
                else if (fromSpace.Letter < toSpace.Letter && fromSpace.Number > toSpace.Number)
                {
                    for (int i = fromSpace.Letter + 1, k = fromSpace.Number - 1; i <= toSpace.Letter && k >= toSpace.Number; i++, k--)
                    {
                        if ((i == toSpace.Letter || k == toSpace.Number))
                        {
                            // capture or move to empty space
                            Console.WriteLine("Space:");
                            fromSpace.PrintInfo();
                            Console.WriteLine();
                            Console.WriteLine("Can capture");
                            toSpace.PrintInfo();
                            return true;
                        }
                        if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
                        {
                            return false;
                        }
                    }
                }
                // if toSpace is up and right
                else if (fromSpace.Letter > toSpace.Letter && fromSpace.Number < toSpace.Number)
                {
                    for (int i = fromSpace.Letter - 1, k = fromSpace.Number + 1; i >= toSpace.Letter && k <= toSpace.Number; i--, k++)
                    {
                        if ((i == toSpace.Letter || k == toSpace.Number))
                        {
                            // capture or move to empty space
                            Console.WriteLine("Space:");
                            fromSpace.PrintInfo();
                            Console.WriteLine();
                            Console.WriteLine("Can capture");
                            toSpace.PrintInfo();
                            return true;
                        }
                        if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
                        {
                            return false;
                        }
                    }
                }
                // if toSpace is up and left
                else if (fromSpace.Letter > toSpace.Letter && fromSpace.Number > toSpace.Number)
                {
                    for (int i = fromSpace.Letter - 1, k = fromSpace.Number - 1; i >= toSpace.Letter && k >= toSpace.Number; i--, k--)
                    {
                        if ((i == toSpace.Letter || k == toSpace.Number))
                        {
                            // capture or move to empty space
                            Console.WriteLine("Space:");
                            fromSpace.PrintInfo();
                            Console.WriteLine();
                            Console.WriteLine("Can capture");
                            toSpace.PrintInfo();
                            return true;
                        }
                        if (Board.spaces[i][k].Piece.belongsToPlayer != Player.None)
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public override bool CanMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return CanTryToCapture(fromSpace, toSpace) &&
                fromSpace.Piece.belongsToPlayer != toSpace.Piece.belongsToPlayer;
        }
    }
}
