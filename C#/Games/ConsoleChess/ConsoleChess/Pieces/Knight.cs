using ConsoleChess.Interfaces;
using ConsoleChess.Enums;

namespace ConsoleChess.Pieces
{
    internal class Knight : Piece
    {
        public Knight(string name, Player belongsTo) : base(name, belongsTo)
        {

        }

        public override bool CanTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            // test whether attempted move follows the rules of chess

            if(fromSpace == toSpace)
            {
                return false;
            }
            if ((Math.Abs(fromSpace.Letter - toSpace.Letter) == 1 && Math.Abs(fromSpace.Number - toSpace.Number) == 2) ||
                (Math.Abs(fromSpace.Number - toSpace.Number) == 1 && Math.Abs(fromSpace.Letter - toSpace.Letter) == 2))
            {
                return true;
            }
            return false;
        }

        public override bool HasPiecesBlockingMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return false;
        }

        public override bool CanTryToCapture(Space fromSpace, Space toSpace)
        {
            return false;
            //if (fromSpace == toSpace)
            //{
            //    return false;
            //}
            //// if one of the differences is 1, and one of the differences is 2... clever!
            //if ((Math.Abs(fromSpace.Letter - toSpace.Letter) == 1 && Math.Abs(fromSpace.Number - toSpace.Number) == 2) ||
            //    (Math.Abs(fromSpace.Number - toSpace.Number) == 1 && Math.Abs(fromSpace.Letter - toSpace.Letter) == 2))
            //{
            //    return true;
            //}
            //return false;
        }

        public override bool CanMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return CanTryToCapture(fromSpace, toSpace) &&
                fromSpace.Piece.belongsToPlayer != toSpace.Piece.belongsToPlayer;
        }
    }
}
