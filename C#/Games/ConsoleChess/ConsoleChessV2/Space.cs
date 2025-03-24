using ConsoleChessV2.Pieces;

namespace ConsoleChessV2
{
    internal class Space
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public Piece? Piece { get; set; }
        public bool IsUnderAttackByWhite;
        public bool IsUnderAttackByBlack;

        public Space(int column, int row, Piece? piece)
        {
            Column = column;
            Row = row;
            Piece = piece;
        }

        public bool IsEmpty()
        {
            return Piece?.BelongsTo == null;
        }

        public bool IsOccupied()
        {
            return Piece?.BelongsTo != null;
        }

        public override string ToString()
        {
            return $"[{Piece?.Name}]";
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Space {Column}, {Row} \n has {Piece} on it and \n" +
                $"Has Moved: {Piece?.HasMoved}\n" +
                $"Is Under Attack by White: {IsUnderAttackByWhite} \n" +
                $"Is Under Attack by Black: {IsUnderAttackByBlack}");

            Console.WriteLine($"White King Space: {ChessBoard.WhiteKingSpace?.Column}" +
                $"{ChessBoard.WhiteKingSpace?.Row}");
            Console.WriteLine($"Black King Space: {ChessBoard.BlackKingSpace?.Column}" +
                $"{ChessBoard.BlackKingSpace?.Column}");
        }

        public void Clear()
        {
            this.Piece = new Piece();
        }
    }
}
