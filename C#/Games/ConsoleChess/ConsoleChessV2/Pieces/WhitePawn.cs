namespace ConsoleChessV2.Pieces
{
    internal class WhitePawn : Piece
    {
        public bool HasJustMovedTwo;
        public WhitePawn()
        {
            Name = "[P]";
            PointValue = 1;
            BelongsTo = Player.White;
        }

        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            if (fromSpace == toSpace)
            {
                return false;
            }
            if (fromSpace.Column == toSpace.Column &&
                fromSpace.Row + 1 == toSpace.Row ||
                (HasMoved == false &&
                fromSpace.Column == toSpace.Column &&
                fromSpace.Row + 2 == toSpace.Row))
            {
                return true;
            }
            return false;
        }

        public override bool CanLegallyTryToCaptureFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            if ((fromSpace.Column - 1 == toSpace.Column && fromSpace.Row + 1 == toSpace.Row) ||
                (fromSpace.Column + 1 == toSpace.Column && fromSpace.Row + 1 == toSpace.Row))
            {
                return true;
            }
            return false;
        }
        public override void CreateListOfPiecesToInspect(Space fromSpace, Space toSpace)
        {
            spacesToMoveToReview?.Clear();
            spacesToCaptureReview?.Clear();
            if (toSpace.Column == fromSpace.Column)
            {
                // moving up
                for (int row = fromSpace.Row + 1; row <= toSpace.Row; row++)
                {
                    spacesToMoveToReview!.Add(ChessBoard.Spaces![fromSpace.Column][row]);
                }
            }
            else if (fromSpace.Column - 1 == toSpace.Column && fromSpace.Row + 1 == toSpace.Row)
            {
                // attacking up and left
                spacesToCaptureReview!.Add(ChessBoard.Spaces![toSpace.Column][toSpace.Row]);
            }
            else if (fromSpace.Column + 1 == toSpace.Column && fromSpace.Row + 1 == toSpace.Row)
            {
                // attacking up and right
                spacesToCaptureReview!.Add(ChessBoard.Spaces![toSpace.Column][toSpace.Row]);
            }
        }

        public override bool CanCaptureFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            // we already know move selection is legal and piece is not blocked
            if (fromSpace.Column != toSpace.Column &&
                       toSpace.Piece?.BelongsTo != null &&
                       fromSpace.Piece?.BelongsTo != toSpace.Piece?.BelongsTo)
            {
                // pawn can capture
                return true;
            }
            // en passant
            // to the left
            if (fromSpace.Column - 1 >= 0)
            {
                if (ChessBoard.Spaces?[fromSpace.Column - 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                {
                    if (fromSpace.Column - 1 == toSpace.Column)
                    {
                        BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece as BlackPawn;
                        // if pawn to the left or right has just moved 2 (hasJustMovedTwo boolean?) allow it to capture that piece, and clear it
                        if (downcast!.HasJustMovedTwo)
                        {
                            //clear black pawn to the left
                            //ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Clear();

                            // capture up and left toward the piece
                            return true;
                        }
                    }
                }
            }
            if (fromSpace.Column + 1 <= 7)
            {
                // to the right
                if (ChessBoard.Spaces?[fromSpace.Column + 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                {
                    if (fromSpace.Column + 1 == toSpace.Column)
                    {
                        BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece as BlackPawn;
                        // if pawn to the left or right has just moved 2 (hasJustMovedTwo boolean?) allow it to capture that piece, and clear it
                        if (downcast!.HasJustMovedTwo)
                        {
                            //clear black pawn to the right
                            //ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Clear();
                            // capture up and right toward the piece
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override Space TryCaptureReturnSpace(Space fromSpace, Space toSpace)
        {
            Piece? tempFromSpacePiece = fromSpace.Piece;
            Piece? tempToSpacePiece = toSpace.Piece;
            if (CanLegallyTryToCaptureFromSpaceToSpace(fromSpace, toSpace))
            {
                if (fromSpace.Piece?.BelongsTo != toSpace.Piece?.BelongsTo)
                {
                    // if a pawn up 2 puts you in check, you are not clearing out the 'fromSpace', you are clearing out the pawn next to your pawn

                    // if en passant
                    // to the left
                    if (fromSpace.Column - 1 >= 0 && ChessBoard.Spaces?[fromSpace.Column - 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                    {
                        if (fromSpace.Column - 1 == toSpace.Column)
                        {
                            BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece as BlackPawn;
                            // if pawn to the left or right has just moved 2 (hasJustMovedTwo boolean?) allow it to capture that piece, and clear it
                            if (downcast!.HasJustMovedTwo)
                            {
                                BlackPawn? tempPawn = ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece as BlackPawn;
                                ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Clear();

                                toSpace.Piece = fromSpace.Piece;
                                fromSpace.Clear();

                                // verify your king is not in check
                                ChessBoard.FindAllSpacesAttacked();
                                if (ChessBoard.turn == Player.White && ChessBoard.WhiteKingSpace!.IsUnderAttackByBlack)
                                {

                                    // cancel move
                                    toSpace.Clear();
                                    fromSpace.Piece = tempFromSpacePiece;
                                    ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece = tempPawn;
                                    return ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row];
                                }
                                else
                                {
                                    // revert move
                                    toSpace.Clear();
                                    fromSpace.Piece = tempFromSpacePiece;
                                    ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece = tempPawn;
                                    return ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row];
                                }
                            }
                        }
                    }
                }
                if (fromSpace.Column + 1 <= 7 && ChessBoard.Spaces?[fromSpace.Column + 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                {
                    // to the right
                    if (fromSpace.Column + 1 == toSpace.Column)
                    {
                        BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece as BlackPawn;
                        // if pawn to the left or right has just moved 2 (hasJustMovedTwo boolean?) allow it to capture that piece, and clear it
                        if (downcast!.HasJustMovedTwo)
                        {
                            toSpace.Piece = fromSpace.Piece;
                            fromSpace.Clear();

                            BlackPawn? tempPawn = ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece as BlackPawn;
                            ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Clear();
                            // verify your king is not in check
                            ChessBoard.FindAllSpacesAttacked();
                            if (ChessBoard.turn == Player.White && ChessBoard.WhiteKingSpace!.IsUnderAttackByBlack)
                            {
                                // cancel move
                                toSpace.Clear();
                                fromSpace.Piece = tempFromSpacePiece;
                                ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece = tempPawn;
                                return ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row];
                            }
                            else
                            {
                                // revert move
                                toSpace.Clear();
                                fromSpace.Piece = tempFromSpacePiece;
                                ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece = tempPawn;
                                return ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row];
                            }
                        }
                    }
                }
                else // if this is not en passant
                {
                    if (toSpace.IsOccupied())
                    {
                        toSpace.Piece = fromSpace.Piece;
                        fromSpace.Clear();
                        // verify your king is not in check
                        if (ChessBoard.EitherKingIsInCheck())
                        {
                            // cancel move
                            fromSpace.Piece = tempFromSpacePiece;
                            toSpace.Piece = tempToSpacePiece;
                            return toSpace;
                        }
                        // revert move
                        fromSpace.Piece = tempFromSpacePiece;
                        toSpace.Piece = tempToSpacePiece;
                        return toSpace;
                    }
                }
            }
            // cancel move
            fromSpace.Piece = tempFromSpacePiece;
            toSpace.Piece = tempToSpacePiece;
            return toSpace;
        }

        public override bool CanMoveFromSpaceToEmptySpace(Space fromSpace, Space toSpace)
        {
            return fromSpace.Column == toSpace.Column &&
                toSpace.Piece?.BelongsTo == null;
        }

        public override void Move(Space fromSpace, Space toSpace)
        {
            if (TryMove(fromSpace, toSpace))
            {
                HasJustMovedTwo = false;
                if (fromSpace.Row + 2 == toSpace.Row)
                {
                    HasJustMovedTwo = true;
                }

                // if promotion
                if (toSpace.Row == 7)
                {
                    Console.WriteLine("Promotion!");

                    Console.WriteLine("Select Piece You Wish to Promote To:");
                    Console.WriteLine("Q: Queen");
                    Console.WriteLine("R: Rook");
                    Console.WriteLine("B: Bishop");
                    Console.WriteLine("N: Knight");

                    string? promotion = Console.ReadLine();

                    switch (promotion)
                    {
                        case "Q":
                            fromSpace.Piece = new WhiteQueen();
                            break;
                        case "R":
                            fromSpace.Piece = new WhiteRook();
                            break;
                        case "B":
                            fromSpace.Piece = new WhiteBishop();
                            break;
                        case "N":
                            fromSpace.Piece = new WhiteKnight();
                            break;
                        default:
                            break;
                    }
                }
                // end promotion

                // if en passant
                // to the left
                if (fromSpace.Column - 1 >= 0)
                {
                    if (ChessBoard.Spaces?[fromSpace.Column - 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                    {
                        if (fromSpace.Column - 1 == toSpace.Column && fromSpace.Row + 1 == toSpace.Row)
                        {
                            // if attacking down and left
                            BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece as BlackPawn;
                            // if pawn to the left has just moved 2, clear it
                            if (downcast!.HasJustMovedTwo)
                            {
                                ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Clear();
                            }
                        }
                    }
                }
                if (fromSpace.Column + 1 <= 7)
                {
                    // to the right
                    if (ChessBoard.Spaces?[fromSpace.Column + 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                    {
                        if (fromSpace.Column + 1 == toSpace.Column && fromSpace.Row + 1 == toSpace.Row)
                        {
                            // if attacking down and right
                            BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece as BlackPawn;
                            // if pawn to the left or right has just moved 2, clear it
                            if (downcast!.HasJustMovedTwo)
                            {
                                ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Clear();
                            }
                        }
                    }
                }
                ChessBoard.AddMoveToHistory(fromSpace, toSpace);

                // finally clear original from space
                toSpace.Piece = fromSpace.Piece;
                toSpace.Piece!.HasMoved = true;
                fromSpace.Clear();

                //ChessBoard.ChangeTurn();
            }
            else if (TryCapture(fromSpace, toSpace))
            {
                ChessBoard.AddMoveToHistory(fromSpace, toSpace);

                // finally clear original from space
                toSpace.Piece = fromSpace.Piece;
                toSpace.Piece!.HasMoved = true;
                fromSpace.Clear();

                //ChessBoard.ChangeTurn();
            }
        }

        public override bool TryCapture(Space fromSpace, Space toSpace)
        {
            if (CanLegallyTryToCaptureFromSpaceToSpace(fromSpace, toSpace))
            {
                Piece? tempFromSpacePiece = fromSpace.Piece;
                Piece? tempToSpacePiece = toSpace.Piece;

                if (fromSpace.Piece?.BelongsTo != toSpace.Piece?.BelongsTo)
                {
                    // if a pawn up 2 puts you in check, you are not clearing out the 'fromSpace', you are clearing out the pawn next to your pawn

                    toSpace.Piece = fromSpace.Piece;
                    fromSpace.Clear();

                    // if en passant
                    // to the left
                    if (fromSpace.Column - 1 >= 0 && ChessBoard.Spaces?[fromSpace.Column - 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                    {
                        if (fromSpace.Column - 1 == toSpace.Column)
                        {
                            BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece as BlackPawn;
                            // if pawn to the left or right has just moved 2 (hasJustMovedTwo boolean?) allow it to capture that piece, and clear it
                            if (downcast!.HasJustMovedTwo)
                            {
                                BlackPawn? tempPawn = ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece as BlackPawn;
                                ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Clear();
                                // verify your king is not in check
                                ChessBoard.FindAllSpacesAttacked();
                                if (ChessBoard.turn == Player.White && ChessBoard.WhiteKingSpace!.IsUnderAttackByBlack)
                                {
                                    // cancel move
                                    toSpace.Clear();
                                    fromSpace.Piece = tempFromSpacePiece;
                                    ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece = tempPawn;
                                    return false;
                                }
                                else
                                {
                                    // revert move
                                    toSpace.Clear();
                                    fromSpace.Piece = tempFromSpacePiece;
                                    ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece = tempPawn;
                                    return true;
                                }
                            }
                        }
                    }
                }
                if (fromSpace.Column + 1 <= 7 && ChessBoard.Spaces?[fromSpace.Column + 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                {
                    // to the right
                    if (fromSpace.Column + 1 == toSpace.Column)
                    {
                        BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece as BlackPawn;
                        // if pawn to the left or right has just moved 2 (hasJustMovedTwo boolean?) allow it to capture that piece, and clear it
                        if (downcast!.HasJustMovedTwo)
                        {
                            BlackPawn? tempPawn = ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece as BlackPawn;
                            ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Clear();
                            // verify your king is not in check
                            ChessBoard.FindAllSpacesAttacked();
                            if (ChessBoard.turn == Player.White && ChessBoard.WhiteKingSpace!.IsUnderAttackByBlack)
                            {
                                // cancel move
                                toSpace.Clear();
                                fromSpace.Piece = tempFromSpacePiece;
                                ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece = tempPawn;
                                return false;
                            }
                            else
                            {
                                // revert move
                                toSpace.Clear();
                                fromSpace.Piece = tempFromSpacePiece;
                                ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece = tempPawn;
                                return true;
                            }
                        }
                    }
                }

                else // if this is not en passant
                {
                    if (tempToSpacePiece?.BelongsTo != null) // if space -was- occupied
                    {
                        // verify your king is not in check
                        if (ChessBoard.EitherKingIsInCheck())
                        {
                            // cancel move
                            fromSpace.Piece = tempFromSpacePiece;
                            toSpace.Piece = tempToSpacePiece;
                            return false;
                        }
                        // revert move
                        fromSpace.Piece = tempFromSpacePiece;
                        toSpace.Piece = tempToSpacePiece;
                        return true;
                    }
                }
                // cancel move
                fromSpace.Piece = tempFromSpacePiece;
                toSpace.Piece = tempToSpacePiece;
                return false;
            }
            return false;
        }

        public override void Capture(Space fromSpace, Space toSpace)
        {
            if (CanLegallyTryToCaptureFromSpaceToSpace(fromSpace, toSpace))
            {
                if (fromSpace.Piece?.BelongsTo != toSpace.Piece?.BelongsTo)
                {
                    Piece? tempFromSpacePiece = fromSpace.Piece;
                    Piece? tempToSpacePiece = toSpace.Piece;

                    // if a pawn up 2 puts you in check, you are not clearing out the 'fromSpace', you are clearing out the pawn next to your pawn

                    toSpace.Piece = fromSpace.Piece;
                    fromSpace.Clear();

                    // if en passant
                    // to the left
                    if (fromSpace.Column - 1 >= 0)
                    {
                        if (ChessBoard.Spaces?[fromSpace.Column - 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                        {
                            if (fromSpace.Column - 1 == toSpace.Column)
                            {
                                BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece as BlackPawn;
                                // if pawn to the left or right has just moved 2 (hasJustMovedTwo boolean?) allow it to capture that piece, and clear it
                                if (downcast!.HasJustMovedTwo)
                                {
                                    BlackPawn? tempPawn = ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece as BlackPawn;
                                    ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Clear();
                                    // verify your king is not in check
                                    ChessBoard.FindAllSpacesAttacked();
                                    if (ChessBoard.turn == Player.White && ChessBoard.WhiteKingSpace!.IsUnderAttackByBlack)
                                    {
                                        // cancel move
                                        toSpace.Clear();
                                        fromSpace.Piece = tempFromSpacePiece;
                                        ChessBoard.Spaces[fromSpace.Column - 1][fromSpace.Row].Piece = tempPawn;
                                    }
                                }
                            }
                        }
                    }
                    if (fromSpace.Column + 1 <= 7)
                    {
                        // to the right
                        if (ChessBoard.Spaces?[fromSpace.Column + 1][fromSpace.Row].Piece!.GetType() == typeof(BlackPawn))
                        {
                            if (fromSpace.Column + 1 == toSpace.Column)
                            {
                                BlackPawn? downcast = ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece as BlackPawn;
                                // if pawn to the left or right has just moved 2 (hasJustMovedTwo boolean?) allow it to capture that piece, and clear it
                                if (downcast!.HasJustMovedTwo)
                                {
                                    BlackPawn? tempPawn = ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece as BlackPawn;
                                    ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Clear();
                                    // verify your king is not in check
                                    ChessBoard.FindAllSpacesAttacked();
                                    if (ChessBoard.turn == Player.White && ChessBoard.WhiteKingSpace!.IsUnderAttackByBlack)
                                    {
                                        // cancel move
                                        toSpace.Clear();
                                        fromSpace.Piece = tempFromSpacePiece;
                                        ChessBoard.Spaces[fromSpace.Column + 1][fromSpace.Row].Piece = tempPawn;
                                    }
                                }
                            }
                        }
                    }
                    else // if this is not en passant
                    {
                        // verify your king is not in check
                        if (ChessBoard.EitherKingIsInCheck())
                        {
                            // cancel move
                            //toSpace.Clear();
                            fromSpace.Piece = tempFromSpacePiece;
                            toSpace.Piece = tempToSpacePiece;
                        }
                    }
                    // verify your king is not in check
                    if (ChessBoard.EitherKingIsInCheck())
                    {
                        // cancel move
                        fromSpace.Piece = tempFromSpacePiece;
                        toSpace.Piece = tempToSpacePiece;
                    }
                }
            }
        }
    }
}
