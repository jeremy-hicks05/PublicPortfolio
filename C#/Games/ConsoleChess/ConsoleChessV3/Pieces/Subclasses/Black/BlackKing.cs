﻿namespace ConsoleChessV3.Pieces.Black
{
    using ConsoleChessV3.Interfaces;
    using ConsoleChessV3.Pieces.Subclasses;
    using static ConsoleChessV3.Enums.Notation;
    internal class BlackKing : King
    {
        public BlackKing()
        {
            Name = "k";
            BelongsTo = Enums.Player.Black;
        }

        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return fromSpace != toSpace &&
                    (Math.Abs(fromSpace.Column - toSpace.Column) <= 1 &&
                    Math.Abs(fromSpace.Row - toSpace.Row) <= 1)
                   ||
                   (ChessBoard.BlackKingSpace is not null &&
                   !ChessBoard.BlackKingSpace.IsUnderAttackByWhite &&
                   ((fromSpace.Column == C["E"] && fromSpace.Row == R["8"] &&
                    toSpace.Column == C["G"] && toSpace.Row == R["8"])
                   ||
                   (fromSpace.Column == C["E"] && fromSpace.Row == R["8"] &&
                    toSpace.Column == C["C"] && toSpace.Row == R["8"])));
        }

        public override void BuildListOfSpacesToInspect(Space fromSpace, Space toSpace)
        {
            SpacesToReview.Clear();
            // if non-castling move
            if (fromSpace.Column - 1 >= 0)
            {
                // attacking left
                if (fromSpace.Column - 1 == toSpace.Column &&
                    fromSpace.Row == toSpace.Row)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column - 1, fromSpace.Row));
                }
            }
            if (fromSpace.Row - 1 >= 0)
            {
                // attacking down
                if (fromSpace.Row - 1 == toSpace.Row &&
                    fromSpace.Column == toSpace.Column)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column, fromSpace.Row - 1));
                }
            }
            if (fromSpace.Row + 1 <= 7)
            {
                // attacking up
                if (fromSpace.Row + 1 == toSpace.Row &&
                    fromSpace.Column == toSpace.Column)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column, fromSpace.Row + 1));
                }
            }
            if (fromSpace.Column + 1 <= 7)
            {
                // attacking right
                if (fromSpace.Column + 1 == toSpace.Column &&
                    fromSpace.Row == toSpace.Row)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column + 1, fromSpace.Row));
                }
            }
            if (fromSpace.Row + 1 <= 7 && fromSpace.Column + 1 <= 7)
            {
                // attacking up and right
                if (fromSpace.Row + 1 == toSpace.Row &&
                    fromSpace.Column + 1 == toSpace.Column)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column + 1, fromSpace.Row + 1));
                }
            }
            if (fromSpace.Row + 1 <= 7 && fromSpace.Column - 1 >= 0)
            {
                // attacking up and left
                if (fromSpace.Row + 1 == toSpace.Row &&
                    fromSpace.Column - 1 == toSpace.Column)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column - 1, fromSpace.Row + 1));
                }
            }
            if (fromSpace.Row - 1 >= 0 && fromSpace.Column + 1 <= 7)
            {
                // attacking down and right
                if (fromSpace.Row - 1 == toSpace.Row &&
                    fromSpace.Column + 1 == toSpace.Column)
                {
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column + 1, fromSpace.Row - 1));
                }
            }
            if (fromSpace.Column - 1 >= 0 && fromSpace.Row - 1 >= 0)
            {
                // attacking down and left
                if (fromSpace.Row - 1 == toSpace.Row &&
                    fromSpace.Column - 1 == toSpace.Column)
                    SpacesToReview.Add(ChessBoard.GetSpace(fromSpace.Column - 1, fromSpace.Row - 1));
            }
            // if castling King side
            if ((HasMoved == false &&
                (ChessBoard.GetSpace(C["H"], R["8"]).GetPiece() is Rook) &&
                ChessBoard.GetSpace(C["H"], R["8"]).GetPiece() is not null &&
                ChessBoard.GetSpace(C["H"], R["8"]).GetPiece()?.GetHasMoved() == false &&
                fromSpace.Row == toSpace.Row &&
                fromSpace.Column == C["E"] && toSpace.Column == C["G"]))
            {
                SpacesToReview.Add(ChessBoard.GetSpace(C["F"], R["8"]));
                SpacesToReview.Add(ChessBoard.GetSpace(C["G"], R["8"]));
            }

            // if castling Queen side
            else if (HasMoved == false &&
                ChessBoard.GetSpace(C["A"], R["8"]).GetPiece() is Rook &&
                ChessBoard.GetSpace(C["A"], R["8"]).GetPiece()?.GetHasMoved() == false &&
                fromSpace.Row == toSpace.Row &&
                fromSpace.Column == C["E"] && toSpace.Column == C["C"])
            {
                SpacesToReview.Add(ChessBoard.GetSpace(C["B"], R["8"]));
                SpacesToReview.Add(ChessBoard.GetSpace(C["C"], R["8"]));
                SpacesToReview.Add(ChessBoard.GetSpace(C["D"], R["8"]));
            }
        }

        public override bool TryMove(Space fromSpace, Space toSpace)
        {
            IPiece? fromSpacePiece = fromSpace.GetPiece();
            if (toSpace.IsEmpty())
            {
                fromSpace.Clear();
                ChessBoard.FindAllSpacesAttacked();
                if (toSpace.IsUnderAttackByWhite)
                {
                    fromSpace.SetPiece(fromSpacePiece);
                    return false;
                }
                fromSpace.SetPiece(fromSpacePiece);
                return true;
            }
            return false;
        }

        public override void Move(Space fromSpace, Space toSpace)
        {
            if (fromSpace.GetPiece() is not null)
            {
                fromSpace.GetPiece()!.SetHasMoved(true);
                toSpace.SetPiece(fromSpace.GetPiece());
                ChessBoard.BlackKingSpace = toSpace;
                fromSpace.Clear();
            }
        }

        public override bool TryCapture(Space fromSpace, Space toSpace)
        {
            if (toSpace.IsOccupied())
            {
                ChessBoard.FindAllSpacesAttacked();
                if (toSpace.IsUnderAttackByWhite)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
