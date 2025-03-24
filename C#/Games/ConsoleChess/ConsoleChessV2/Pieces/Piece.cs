namespace ConsoleChessV2.Pieces
{
    internal class Piece
    {
        public string? Name { get; set; }
        public int PointValue { get; set; }
        public Player? BelongsTo { get; set; }
        public List<Space>? spacesToMoveToReview = new();
        public List<Space>? spacesToCaptureReview = new();
        public bool HasMoved { get; set; }

        public Piece()
        {
            Name = "[ ]";
            PointValue = 0;
            BelongsTo = null;
        }

        public virtual void CreateListOfPiecesToInspect(Space fromSpace, Space toSpace)
        {
            //Console.WriteLine("Calling wrong method!");
        }

        public virtual bool CanCaptureFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            // we already know move selection is legal and piece is not blocked
            return toSpace.Piece?.BelongsTo != null &&
                fromSpace.Piece?.BelongsTo != toSpace.Piece?.BelongsTo;
        }

        public virtual bool CanMoveFromSpaceToEmptySpace(Space fromSpace, Space toSpace)
        {
            return toSpace.Piece?.BelongsTo == null;
        }

        public virtual bool TryMove(Space fromSpace, Space toSpace)
        {
            if (CanLegallyTryToMoveFromSpaceToSpace(fromSpace, toSpace) &&
                !IsBlocked(fromSpace, toSpace) &&
                toSpace.IsEmpty())
            {
                Piece? tempFromSpacePiece = fromSpace.Piece;
                Piece? tempToSpacePiece = toSpace.Piece;

                toSpace.Piece = fromSpace.Piece;
                fromSpace.Clear();

                // verify your king is not in check
                if (ChessBoard.EitherKingIsInCheck())
                {
                    // cancel move
                    fromSpace.Piece = tempFromSpacePiece;
                    toSpace.Piece = tempToSpacePiece;
                    return false;
                }
                // revert move and let calling function finish it
                fromSpace.Piece = tempFromSpacePiece;
                toSpace.Piece = tempToSpacePiece;
                return true;
            }
            return false;
        }

        public virtual void ChessMove(Space fromSpace, Space toSpace)
        {
            if (ChessBoard.turn == fromSpace.Piece?.BelongsTo)
            {
                Piece? startingPiece = fromSpace.Piece;

                Space? changedSpace = startingPiece?.TryCaptureReturnSpace(fromSpace, toSpace); // store changed space for en passant
                Piece? changedSpacePiece = changedSpace!.Piece;

                bool startingPieceHasMoved = startingPiece!.HasMoved;
                bool changedSpacePieceHasMoved = changedSpacePiece!.HasMoved;

                if (TryMove(fromSpace, toSpace))
                {
                    Move myMove = new Move(
                        initiatingSpace: fromSpace,
                        initiatingPiece: startingPiece,
                        targetSpace: toSpace,
                        targetPiece: toSpace.Piece,
                        affectedSpace: changedSpace,
                        affectedPiece: changedSpacePiece
                        );
                    // changedSpace1 == H8 and changedSpace2 == F8
                    ChessBoard.MovesPlayed.Push(myMove);
                    Move(fromSpace, toSpace);
                    ChessBoard.ChangeTurn();
                }
                else if (TryCapture(fromSpace, toSpace))
                {
                    Move myMove = new Move(
                        initiatingSpace: fromSpace,
                        initiatingPiece: startingPiece,
                        targetSpace: toSpace,
                        targetPiece: toSpace.Piece,
                        affectedSpace: changedSpace,
                        affectedPiece: changedSpacePiece
                        );
                    ChessBoard.MovesPlayed.Push(myMove);
                    Capture(fromSpace, toSpace);
                    ChessBoard.ChangeTurn();
                }
            }
        }

        public virtual void Move(Space fromSpace, Space toSpace)
        {
            toSpace.Piece = fromSpace.Piece;
            toSpace.Piece!.HasMoved = true;
            fromSpace.Clear();
        }

        public virtual void Capture(Space fromSpace, Space toSpace)
        {
            toSpace.Piece = fromSpace.Piece;
            toSpace.Piece!.HasMoved = true;
            fromSpace.Clear();
        }

        public virtual Space TryCaptureReturnSpace(Space fromSpace, Space toSpace)
        {
            if (CanLegallyTryToCaptureFromSpaceToSpace(fromSpace, toSpace))
            {
                Piece? tempFromSpacePiece = fromSpace.Piece;
                Piece? tempToSpacePiece = toSpace.Piece;

                // if a pawn up 2 puts you in check, you are not clearing out the 'fromSpace', you are clearing out the pawn next to your pawn

                toSpace.Piece = fromSpace.Piece;
                fromSpace.Clear();
                toSpace.Clear();
                ChessBoard.FindAllSpacesAttacked();

                // verify your king is not in check
                if (ChessBoard.turn == Player.White && ChessBoard.WhiteKingSpace!.IsUnderAttackByBlack)
                {
                    // cancel move
                    fromSpace.Piece = tempFromSpacePiece;
                    toSpace.Piece = tempToSpacePiece;
                    return fromSpace;
                }
                else if (ChessBoard.turn == Player.Black && ChessBoard.BlackKingSpace!.IsUnderAttackByWhite)
                {
                    // cancel move
                    fromSpace.Piece = tempFromSpacePiece;
                    toSpace.Piece = tempToSpacePiece;
                    return fromSpace;
                }
                // revert capture, let calling function finish capture
                fromSpace.Piece = tempFromSpacePiece;
                toSpace.Piece = tempToSpacePiece;
                return toSpace;
            }
            // cancel move
            return fromSpace;
        }

        public virtual bool TryCapture(Space fromSpace, Space toSpace)
        {
            if (CanLegallyTryToCaptureFromSpaceToSpace(fromSpace, toSpace) &&
                !IsBlocked(fromSpace, toSpace) &&
                toSpace.Piece?.BelongsTo != fromSpace.Piece?.BelongsTo)
            {
                Piece? tempFromSpacePiece = fromSpace.Piece;
                Piece? tempToSpacePiece = toSpace.Piece;

                toSpace.Piece = fromSpace.Piece;
                fromSpace.Clear();

                // verify your king is not in check
                if (ChessBoard.EitherKingIsInCheck())
                {
                    // cancel move
                    fromSpace.Piece = tempFromSpacePiece;
                    toSpace.Piece = tempToSpacePiece;
                    return false;
                }
                // revert move and let calling function finish it
                fromSpace.Piece = tempFromSpacePiece;
                toSpace.Piece = tempToSpacePiece;
                return true;
            }
            return false;
        }

        public virtual bool IsBlocked(Space fromSpace, Space toSpace)
        {
            fromSpace.Piece?.CreateListOfPiecesToInspect(fromSpace, toSpace); // added
            // move options
            foreach (Space s in fromSpace.Piece?.spacesToMoveToReview!)
            {
                if (s != fromSpace.Piece.spacesToMoveToReview.Last())
                {
                    if (s.IsOccupied())
                    {
                        // piece is blocked
                        return true;
                    }
                }
                if (s == fromSpace.Piece.spacesToMoveToReview.Last())
                {
                    // piece is not blocked
                    return false;
                }
            }

            foreach (Space s in fromSpace.Piece?.spacesToCaptureReview!)
            {
                if (s != fromSpace.Piece.spacesToCaptureReview.Last())
                {
                    if (s.IsOccupied())
                    {
                        // piece is blocked
                        return true;
                    }
                }
                if (s == fromSpace.Piece.spacesToCaptureReview.Last())
                {
                    // piece is not blocked
                    return false;
                }
            }
            return true;
        }

        public virtual bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            //Console.WriteLine("Calling wrong method!");
            return false;
        }

        public virtual bool CanLegallyTryToCaptureFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return CanLegallyTryToMoveFromSpaceToSpace(fromSpace, toSpace) &&
                    !fromSpace.Piece!.IsBlocked(fromSpace, toSpace);
        }
    }
}
