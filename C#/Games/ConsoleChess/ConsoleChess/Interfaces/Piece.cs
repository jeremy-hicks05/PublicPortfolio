using ConsoleChess.Enums;

namespace ConsoleChess.Interfaces
{
    internal class Piece
    {
        public string Name { get; set; }
        public Player belongsToPlayer = Player.None;
        public bool hasMoved;

        public Piece(string name, Player belongsTo)
        {
            Name = name;
            belongsToPlayer = belongsTo;
            hasMoved = false;
        }

        public virtual Space[]? GetSpacesAvaiableToMoveTo()
        {
            // return array of spaces particular piece can move to
            return null;
        }

        public virtual bool CanTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return false;
        }

        public virtual bool HasPiecesBlockingMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return true;
        }

        public virtual bool CanMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return false;
        }

        public virtual bool CanTryToCapture(Space fromSpace, Space toSpace)
        {
            return false;
        }

        public virtual bool CanCaptureFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return fromSpace.Piece.belongsToPlayer != toSpace.Piece.belongsToPlayer;
        }
    }
}
