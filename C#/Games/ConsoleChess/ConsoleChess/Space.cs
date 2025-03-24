using ConsoleChess.Interfaces;
using ConsoleChess.Enums;

namespace ConsoleChess
{
    internal class Space
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public Piece Piece { get; set; }

        public bool IsUnderAttackByBlack = false;
        public bool IsUnderAttackByWhite = false;

        public Space()
        {
            Row = -1;
            Column = -1;
            Piece = new Piece("[ ]", Player.None);
        }

        public void PrintInfo()
        {
            Console.WriteLine("Piece: " + Piece);
            Console.WriteLine("Controlled by: " + Piece.belongsToPlayer);
            Console.WriteLine("Space: " + Row + ", " + Column);
            Console.WriteLine("Is under attack by White: " + IsUnderAttackByWhite);
            Console.WriteLine("Is under attack by Black: " + IsUnderAttackByWhite);
        }

        public string PrintSpace()
        {
            if(Piece == null)
            {
                return "[ ]";
            }
            else
            {
                return Piece.Name;
            }
        }

        public void SetSpace(Space space)
        {
            Row = space.Row;
            Column = space.Column;
            Piece = space.Piece;
            IsUnderAttackByBlack = space.IsUnderAttackByBlack;
            IsUnderAttackByWhite = space.IsUnderAttackByWhite;
        }
    }
}
