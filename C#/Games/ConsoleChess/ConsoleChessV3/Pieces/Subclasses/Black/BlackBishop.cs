using ConsoleChessV3.Pieces.Subclasses;

namespace ConsoleChessV3.Pieces.Black
{
    internal class BlackBishop : Bishop
    {
        public BlackBishop()
        {
            Name = "b";
            BelongsTo = Enums.Player.Black;
        }
    }
}
