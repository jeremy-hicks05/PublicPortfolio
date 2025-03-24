using ConsoleChessV3.Pieces.Subclasses;

namespace ConsoleChessV3.Pieces.Black
{
    internal class BlackRook : Rook
    {
        public BlackRook()
        {
            Name = "r";
            BelongsTo = Enums.Player.Black;
        }
    }
}
