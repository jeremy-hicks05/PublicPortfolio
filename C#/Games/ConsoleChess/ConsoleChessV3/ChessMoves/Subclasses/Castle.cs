namespace ConsoleChessV3.ChessMoves.Subclasses
{
    using ConsoleChessV3.ChessMoves;
    using ConsoleChessV3.Pieces.White;
    using static ConsoleChessV3.Enums.Notation;
    internal class Castle : ChessMove
    {
        public Castle(Space startingSpace, Space endingSpace) : base(startingSpace, endingSpace)
        {
            // add designation for 'captured' / 'affected' piece(s)?
            if (StartingSpace.Column + 2 == TargetSpace.Column) // king side castle
            {
                RestorePiece = ChessBoard.GetSpace(C["H"], StartingSpace.Row).GetPiece();
                RestoreSpace = ChessBoard.GetSpace(C["H"], StartingSpace.Row);
            }
            else // queen side castle
            {
                RestorePiece = ChessBoard.GetSpace(C["A"], StartingSpace.Row).GetPiece();
                RestoreSpace = ChessBoard.GetSpace(C["A"], StartingSpace.Row);
            }

        }

        public override void Perform()
        {
            if (StartingSpace.Column + 2 == TargetSpace.Column) // king side castle
            {
                ChessBoard.GetSpace(C["G"], StartingSpace.Row).SetPiece(ChessBoard.GetSpace(C["E"], StartingSpace.Row).GetPiece());
                ChessBoard.GetSpace(C["F"], StartingSpace.Row).SetPiece(ChessBoard.GetSpace(C["H"], StartingSpace.Row).GetPiece());

                ChessBoard.GetSpace(C["E"], StartingSpace.Row).Clear();
                ChessBoard.GetSpace(C["H"], StartingSpace.Row).Clear();

                if (ChessBoard.Turn == Enums.Player.White)
                {
                    ChessBoard.WhiteKingSpace = TargetSpace;
                }
                else
                {
                    ChessBoard.BlackKingSpace = TargetSpace;
                }

            }
            else // Queen side castle
            {
                ChessBoard.GetSpace(C["C"], StartingSpace.Row).SetPiece(ChessBoard.GetSpace(C["E"], StartingSpace.Row).GetPiece());
                ChessBoard.GetSpace(C["D"], StartingSpace.Row).SetPiece(ChessBoard.GetSpace(C["A"], StartingSpace.Row).GetPiece());

                ChessBoard.GetSpace(C["E"], StartingSpace.Row).Clear();
                ChessBoard.GetSpace(C["A"], StartingSpace.Row).Clear();

                if (ChessBoard.Turn == Enums.Player.White)
                {
                    ChessBoard.WhiteKingSpace = TargetSpace;
                }
                else
                {
                    ChessBoard.BlackKingSpace = TargetSpace;
                }
            }
        }

        public override bool IsValidChessMove()
        {
            if (StartingSpace.Column + 2 == TargetSpace.Column) // king side castle
            {
                return ChessBoard.GetSpace(C["F"], StartingSpace.Row).IsEmpty() &&
                    ChessBoard.GetSpace(C["G"], StartingSpace.Row).IsEmpty()
                    &&
                    !(ChessBoard.GetSpace(C["E"], StartingSpace.Row).IsUnderAttackByOpponent() ||
                    ChessBoard.GetSpace(C["F"], StartingSpace.Row).IsUnderAttackByOpponent() ||
                    ChessBoard.GetSpace(C["G"], StartingSpace.Row).IsUnderAttackByOpponent());
            }
            else // queen side castle
            {
                return ChessBoard.GetSpace(C["B"], StartingSpace.Row).IsEmpty() &&
                    ChessBoard.GetSpace(C["C"], StartingSpace.Row).IsEmpty() &&
                    ChessBoard.GetSpace(C["D"], StartingSpace.Row).IsEmpty()
                    &&
                    !(ChessBoard.GetSpace(C["E"], StartingSpace.Row).IsUnderAttackByOpponent() ||
                    ChessBoard.GetSpace(C["D"], StartingSpace.Row).IsUnderAttackByOpponent() ||
                    ChessBoard.GetSpace(C["C"], StartingSpace.Row).IsUnderAttackByOpponent());
            }
        }

        public override void Reverse()
        {
            StartingSpace.SetPiece(StartingPiece);
            StartingSpace.GetPiece()!.SetHasMoved(StartingPieceHasMoved);

            if (ChessBoard.Turn == Enums.Player.White)
            {
                // ChangeTurn happens afterwards
                ChessBoard.BlackKingSpace = StartingSpace;
            }
            else
            {
                // ChangeTurn happens afterwards
                ChessBoard.WhiteKingSpace = StartingSpace;
            }

            if (RestoreSpace is not null)
            {
                RestoreSpace.SetPiece(RestorePiece);
            }
            if (RestoreSpace is not null && RestoreSpace.GetPiece() is not null)
            {
                RestoreSpace.GetPiece()!.SetHasMoved(RestorePieceHasMoved);
            }


            if (StartingSpace.Column + 2 == TargetSpace.Column)
            {
                ChessBoard.GetSpace(StartingSpace.Column + 1, StartingSpace.Row).Clear();
                ChessBoard.GetSpace(StartingSpace.Column + 2, StartingSpace.Row).Clear();
            }
            else
            {
                ChessBoard.GetSpace(StartingSpace.Column - 1, StartingSpace.Row).Clear();
                ChessBoard.GetSpace(StartingSpace.Column - 2, StartingSpace.Row).Clear();
            }
        }
    }
}
