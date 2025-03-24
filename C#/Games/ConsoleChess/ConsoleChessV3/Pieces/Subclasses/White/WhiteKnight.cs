using ConsoleChessV3.Pieces.Subclasses;

namespace ConsoleChessV3.Pieces.White
{
    internal class WhiteKnight : Knight
    {
        public WhiteKnight()
        {
            Name = "N";
            BelongsTo = Enums.Player.White;
        }
    }
}
