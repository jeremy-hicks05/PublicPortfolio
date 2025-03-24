using ConsoleChessV3.Pieces.Subclasses;

namespace ConsoleChessV3.Pieces.White
{
    internal class WhiteBishop : Bishop
    {
        public WhiteBishop()
        {
            Name = "B";
            BelongsTo = Enums.Player.White;
        }
    }
}
