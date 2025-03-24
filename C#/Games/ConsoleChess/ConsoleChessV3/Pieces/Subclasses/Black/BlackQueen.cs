using ConsoleChessV3.Pieces.Subclasses;

namespace ConsoleChessV3.Pieces.Black
{
    internal class BlackQueen : Queen
    {
        public BlackQueen()
        {
            Name = "q";
            BelongsTo = Enums.Player.Black;
        }
    }
}
