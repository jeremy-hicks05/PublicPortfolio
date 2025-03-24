using ConsoleChessV3.Enums;
using ConsoleChessV3.Interfaces;

namespace ConsoleChessV3
{
    internal class Space
    {
        public int Column;
        public int Row;
        private IPiece? Piece;

        public bool IsUnderAttackByWhite;
        public bool IsUnderAttackByBlack;

        public Space()
        {
            Column = -1;
            Row = -1;
            Piece = null;
        }

        public override string ToString()
        {
            return (Piece == null ? $"[ ]" : $"[{Piece.GetName()}]");
        }

        public IPiece? GetPiece()
        {
            if(Piece == null)
            {
                return null;
            }
            return Piece;
        }

        public void SetPiece(IPiece? piece)
        {
            Piece = piece;
        }

        public void Clear()
        {
            this.Piece = null;
        }

        /// <summary>
        /// Returns whether Space has a piece on it
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Piece == null;
        }

        /// <summary>
        /// Returns whether Space has a piece on it
        /// </summary>
        /// <returns></returns>
        public bool IsOccupied()
        {
            return Piece != null;
        }

        public bool HasWhitePieceOnIt()
        {
            return (Piece is not null) && (Piece.GetBelongsTo() == Player.White);
        }

        public bool HasBlackPieceOnIt()
        {
            return (Piece is not null) && (Piece.GetBelongsTo() == Player.Black);
        }

        /// <summary>
        /// Returns whether this Space is under attack by one of the opponent's pieces
        /// </summary>
        /// <returns></returns>
        public bool IsUnderAttackByOpponent()
        {
            switch (ChessBoard.Turn)
            {
                case Player.Black:
                    return IsUnderAttackByWhite;
                default:
                    return IsUnderAttackByBlack;
            }
        }

        public string PrintNotation()
        {
            string notation = "";
            switch (Column)
            {
                case 0:
                    notation = "a";
                    break;
                case 1:
                    notation = "b";
                    break;
                case 2:
                    notation = "c";
                    break;
                case 3:
                    notation = "d";
                    break;
                case 4:
                    notation = "e";
                    break;
                case 5:
                    notation = "f";
                    break;
                case 6:
                    notation = "g";
                    break;
                case 7:
                    notation = "h";
                    break;
                default:
                    break;
            }

            switch(Row)
            {
                case 0:
                    notation += "1";
                    break;
                case 1:
                    notation += "2";
                    break;
                case 2:
                    notation += "3";
                    break;
                case 3:
                    notation += "4";
                    break;
                case 4:
                    notation += "5";
                    break;
                case 5:
                    notation += "6";
                    break;
                case 6:
                    notation += "7";
                    break;
                case 7:
                    notation += "8";
                    break;
                default:
                    break;
            }
            return notation;
        }
    }
}
