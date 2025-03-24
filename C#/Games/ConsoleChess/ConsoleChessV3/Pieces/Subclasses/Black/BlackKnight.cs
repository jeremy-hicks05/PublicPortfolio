using ConsoleChessV3.Pieces.Subclasses;

namespace ConsoleChessV3.Pieces.Black
{
    internal class BlackKnight : Knight
    {
        public BlackKnight()
        {
            Name = "n";
            BelongsTo = Enums.Player.Black;
        }
    }
}
