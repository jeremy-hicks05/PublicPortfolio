﻿namespace ConsoleChessV2.Pieces
{
    using static Notation;
    internal class BlackKing : Piece
    {
        public BlackKing()
        {
            Name = "[k]";
            PointValue = 99;
            BelongsTo = Player.Black;
        }

        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            if (fromSpace == toSpace)
            {
                return false;
            }
            if ((Math.Abs(fromSpace.Column - toSpace.Column) <= 1 &&
                Math.Abs(fromSpace.Row - toSpace.Row) <= 1))
            {
                return true;
            }
            if (HasMoved == false &&
                fromSpace.Row == toSpace.Row &&
                fromSpace.Column == C["E"] && toSpace.Column == C["G"])
            {
                // attemping to castle King Side
                return true;
            }

            if (HasMoved == false &&
                fromSpace.Row == toSpace.Row &&
                fromSpace.Column == C["E"] && toSpace.Column == C["C"])
            {
                // attemping to castle Queen Side
                return true;
            }
            return false;
        }

        public override void CreateListOfPiecesToInspect(Space fromSpace, Space toSpace)
        {
            spacesToMoveToReview?.Clear();

            // if non-castling move
            if (fromSpace.Column - 1 >= 0)
            {
                // attacking left
                if (fromSpace.Column - 1 == toSpace.Column &&
                    fromSpace.Row == toSpace.Row)
                {
                    spacesToMoveToReview?.Add(ChessBoard.Spaces![fromSpace.Column - 1][fromSpace.Row]);
                }
            }
            if (fromSpace.Row - 1 >= 0)
            {
                // attacking down
                if (fromSpace.Row - 1 == toSpace.Row &&
                    fromSpace.Column == toSpace.Column)
                {
                    spacesToMoveToReview?.Add(ChessBoard.Spaces![fromSpace.Column][fromSpace.Row - 1]);
                }
            }
            if (fromSpace.Row + 1 <= 7)
            {
                // attacking up
                if (fromSpace.Row + 1 == toSpace.Row &&
                    fromSpace.Column == toSpace.Column)
                {
                    spacesToMoveToReview?.Add(ChessBoard.Spaces![fromSpace.Column][fromSpace.Row + 1]);
                }
            }
            if (fromSpace.Column + 1 <= 7)
            {
                // attacking right
                if (fromSpace.Column + 1 == toSpace.Column &&
                    fromSpace.Row == toSpace.Row)
                {
                    spacesToMoveToReview?.Add(ChessBoard.Spaces![fromSpace.Column + 1][fromSpace.Row]);
                }
            }
            if (fromSpace.Row + 1 <= 7 && fromSpace.Column + 1 <= 7)
            {
                // attacking up and right
                if (fromSpace.Row + 1 == toSpace.Row &&
                    fromSpace.Column + 1 == toSpace.Column)
                {
                    spacesToMoveToReview?.Add(ChessBoard.Spaces![fromSpace.Column + 1][fromSpace.Row + 1]);
                }
            }
            if (fromSpace.Row + 1 <= 7 && fromSpace.Column - 1 >= 0)
            {
                // attacking up and left
                if (fromSpace.Row + 1 == toSpace.Row &&
                    fromSpace.Column - 1 == toSpace.Column)
                {
                    spacesToMoveToReview?.Add(ChessBoard.Spaces![fromSpace.Column - 1][fromSpace.Row + 1]);
                }
            }
            if (fromSpace.Row - 1 >= 0 && fromSpace.Column + 1 <= 7)
            {
                // attacking down and right
                if (fromSpace.Row - 1 == toSpace.Row &&
                    fromSpace.Column + 1 == toSpace.Column)
                {
                    spacesToMoveToReview?.Add(ChessBoard.Spaces![fromSpace.Column + 1][fromSpace.Row - 1]);
                }
            }
            if (fromSpace.Column - 1 >= 0 && fromSpace.Row - 1 >= 0)
            {
                // attacking down and left
                if (fromSpace.Row - 1 == toSpace.Row &&
                    fromSpace.Column - 1 == toSpace.Column)
                    spacesToMoveToReview?.Add(ChessBoard.Spaces![fromSpace.Column - 1][fromSpace.Row - 1]);
            }
            // if castling King side
            if (HasMoved == false &&
                ChessBoard.Spaces![C["H"]][R["8"]].Piece?.HasMoved == false &&
                fromSpace.Row == toSpace.Row &&
                fromSpace.Column == C["E"] && toSpace.Column == C["G"])
            {
                spacesToMoveToReview?.Add(ChessBoard.Spaces![C["F"]][R["8"]]);
                spacesToMoveToReview?.Add(ChessBoard.Spaces![C["G"]][R["8"]]);
            }

            // if castling Queen side
            else if (HasMoved == false &&
                ChessBoard.Spaces![C["A"]][R["8"]].Piece?.HasMoved == false &&
                fromSpace.Row == toSpace.Row &&
                fromSpace.Column == C["E"] && toSpace.Column == C["C"])
            {
                spacesToMoveToReview?.Add(ChessBoard.Spaces![C["B"]][R["8"]]);
                spacesToMoveToReview?.Add(ChessBoard.Spaces![C["C"]][R["8"]]);
                spacesToMoveToReview?.Add(ChessBoard.Spaces![C["D"]][R["8"]]);
            }
        }

        public override bool IsBlocked(Space fromSpace, Space toSpace)
        {
            fromSpace.Piece?.CreateListOfPiecesToInspect(fromSpace, toSpace);
            // move options
            foreach (Space s in fromSpace.Piece?.spacesToMoveToReview!)
            {
                if (s != fromSpace.Piece.spacesToMoveToReview.Last())
                {
                    if (s.Piece?.BelongsTo != null || s.IsUnderAttackByWhite)
                    {
                        // piece is blocked or cannot castle
                        return true;
                    }
                }
                if (s == fromSpace.Piece.spacesToMoveToReview.Last())
                {
                    // piece is not blocked
                    return false;
                }
            }
            return true;
        }

        public override bool TryMove(Space fromSpace, Space toSpace)
        {
            if (CanLegallyTryToMoveFromSpaceToSpace(fromSpace, toSpace) && 
                !(IsBlocked(fromSpace, toSpace)) && 
                fromSpace.Piece?.BelongsTo != toSpace.Piece?.BelongsTo)
            {
                Piece? tempFromSpacePiece = fromSpace.Piece;
                Piece? tempToSpacePiece = toSpace.Piece;

                // if castling
                if (Math.Abs(toSpace.Column - fromSpace.Column) == 2)
                {
                    return CanCastle(fromSpace, toSpace);
                }

                // move king's designated space
                ChessBoard.BlackKingSpace = toSpace;
                toSpace.Piece = fromSpace.Piece;
                fromSpace.Clear();

                // verify your king is not in check
                if (ChessBoard.EitherKingIsInCheck())
                {
                    // cancel move
                    fromSpace.Piece = tempFromSpacePiece;
                    toSpace.Piece = tempToSpacePiece;

                    ChessBoard.BlackKingSpace = fromSpace;
                    return false;
                }

                // revert move -> let calling function perform move
                fromSpace.Piece = tempFromSpacePiece;
                toSpace.Piece = tempToSpacePiece;

                // restore king's location
                ChessBoard.BlackKingSpace = fromSpace;
                return true;
            }
            return false;
        }

        public override void Move(Space fromSpace, Space toSpace)
        {
            if (TryMove(fromSpace, toSpace))
            {
                // check for castle and update KingSpace
                if (fromSpace.Column + 2 == toSpace.Column)
                {
                    // castle king side white
                    ChessBoard.Spaces![C["F"]][R["8"]].Piece = ChessBoard.Spaces[C["H"]][R["8"]].Piece;
                    ChessBoard.Spaces[C["H"]][R["8"]].Clear();
                }
                if (fromSpace.Column - 2 == toSpace.Column)
                {
                    // castle queen side white
                    ChessBoard.Spaces![C["D"]][R["8"]].Piece = ChessBoard.Spaces[C["A"]][R["8"]].Piece;
                    ChessBoard.Spaces[C["A"]][R["8"]].Clear();
                }
                toSpace.Piece = fromSpace.Piece;
                ChessBoard.BlackKingSpace = toSpace;
                toSpace.Piece!.HasMoved = true;
                fromSpace.Clear();

                //ChessBoard.ChangeTurn();
            }
        }

        // specialized King functions

        public bool CanCastle(Space fromSpace, Space toSpace)
        {
            fromSpace.Piece?.CreateListOfPiecesToInspect(fromSpace, toSpace);

            // castle options
            if ((fromSpace.IsUnderAttackByWhite))
            {
                return false;
            }
            foreach (Space s in fromSpace.Piece?.spacesToMoveToReview!)
            {
                if (s.IsUnderAttackByWhite)
                {
                    // castle is "blocked"
                    return false;
                }

                if (s == fromSpace.Piece.spacesToMoveToReview.Last() && !(s.IsUnderAttackByWhite))
                {
                    // castle is not "blocked"
                    return true;
                }
            }
            return false;
        }

        public override bool TryCapture(Space fromSpace, Space toSpace)
        {
            if (CanLegallyTryToCaptureFromSpaceToSpace(fromSpace, toSpace) && !(IsBlocked(fromSpace, toSpace)) && toSpace.Piece?.BelongsTo == Player.White)
            {
                Piece? tempFromSpacePiece = fromSpace.Piece;
                Piece? tempToSpacePiece = toSpace.Piece;

                ChessBoard.BlackKingSpace = toSpace;
                toSpace.Piece = fromSpace.Piece;

                fromSpace.Clear();
                toSpace.Clear();

                ChessBoard.FindAllSpacesAttacked();

                // verify your king is not in check
                if (ChessBoard.BlackKingSpace!.IsUnderAttackByWhite)
                {
                    // cancel move
                    fromSpace.Piece = tempFromSpacePiece;
                    toSpace.Piece = tempToSpacePiece;

                    ChessBoard.BlackKingSpace = fromSpace;
                    return false;
                }
                fromSpace.Piece = tempFromSpacePiece;
                toSpace.Piece = tempToSpacePiece;

                ChessBoard.BlackKingSpace = fromSpace;
                return true;
            }
            return false;
        }
    }
}
