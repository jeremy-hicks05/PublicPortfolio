using ConsoleChessV3.Pieces.Subclasses;
using ConsoleChessV3.ChessMoves;
using ConsoleChessV3.ChessMoves.Subclasses;
using ConsoleChessV3.Pieces.Black;
using ConsoleChessV3.Pieces.White;
using ConsoleChessV3.Enums;

namespace ConsoleChessV3.Builders
{
    internal static class MoveBuilder
    {
        /// <summary>
        /// Builds a ChessMove, determining which type it is (EnPassant, Promotion, Capture, Move, Castle)
        /// and returns it to the calling function for future performing
        /// </summary>
        /// <param name="fromSpace"></param>
        /// <param name="toSpace"></param>
        /// <returns></returns>
        public static ChessMove? Build(Space fromSpace, Space toSpace)
        {
            if (fromSpace.GetPiece() is not null)
            {
                if (IsEnPassant(fromSpace, toSpace))
                {
                    return new EnPassant(fromSpace, toSpace);
                }
                else if (IsPromotion(fromSpace, toSpace))
                {
                    return new Promotion(fromSpace, toSpace);
                }
                else if (IsCastle(fromSpace, toSpace))
                {
                    return new Castle(fromSpace, toSpace);
                }
                else if(IsCapture(fromSpace, toSpace))
                {
                    return new Capture(fromSpace, toSpace);
                }
                else if(IsMove(fromSpace, toSpace))
                {
                    return new Move(fromSpace, toSpace);
                }
                else
                {
                    throw new Exception("Move error");
                }
            }
            return null;
        }

        private static bool IsEnPassant(Space fromSpace, Space toSpace)
        {
            if (ChessBoard.Turn == Enums.Player.White &&
                    fromSpace.GetPiece() is Pawn &&
                    toSpace.GetPiece() is null &&
                    (Math.Abs(toSpace.Column - fromSpace.Column) == 1))
            {
                if (ChessBoard.GetSpace(toSpace.Column, toSpace.Row - 1).GetPiece() is BlackPawn)
                {
                    Pawn? tempPawn = ChessBoard.GetSpace(toSpace.Column, toSpace.Row - 1).GetPiece() as Pawn;
                    if (tempPawn is not null && tempPawn.HasJustMovedTwo)
                    {
                        if (ChessBoard.GetSpace(toSpace.Column, toSpace.Row - 1).GetPiece()!.GetBelongsTo() != fromSpace.GetPiece()!.GetBelongsTo())
                        {
                            return true;
                        }
                    }
                }
            }
            // check for Black EnPassant move
            else if (ChessBoard.Turn == Enums.Player.Black &&
                fromSpace.GetPiece() is Pawn &&
                toSpace.GetPiece() is null &&
                (Math.Abs(toSpace.Column - fromSpace.Column) == 1))
            {
                if (ChessBoard.GetSpace(toSpace.Column, toSpace.Row + 1).GetPiece() is WhitePawn)
                {
                    Pawn? tempPawn = ChessBoard.GetSpace(toSpace.Column, toSpace.Row + 1).GetPiece() as Pawn;
                    if (tempPawn is not null && tempPawn.HasJustMovedTwo)
                    {
                        if (ChessBoard.GetSpace(toSpace.Column, toSpace.Row + 1).GetPiece()!.GetBelongsTo() != fromSpace.GetPiece()!.GetBelongsTo())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool IsPromotion(Space fromSpace, Space toSpace)
        {
            return (fromSpace.GetPiece() is Pawn && toSpace.Row == 7) ||
                    (fromSpace.GetPiece() is Pawn && toSpace.Row == 0);
        }
        private static bool IsCapture(Space fromSpace, Space toSpace)
        {
            return toSpace.IsOccupied();
        }

        private static bool IsCastle(Space fromSpace, Space toSpace)
        {
            return (fromSpace.GetPiece() is King &&
                    Math.Abs(fromSpace.Column - toSpace.Column) == 2);
        }

        private static bool IsMove(Space fromSpace, Space toSpace)
        {
            return toSpace.IsEmpty();
        }
    }
}
