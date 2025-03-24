

namespace ConsoleChessV2
{
    using ConsoleChessV2.Moves;
    using ConsoleChessV2.Pieces;
    using System.Text.RegularExpressions;
    using static Notation;
    internal static class ChessBoard
    {
        public static Space[][]? Spaces { get; set; }
        public static Space? WhiteKingSpace { get; set; }
        public static Space? BlackKingSpace { get; set; }

        public static Stack<Move> MovesPlayed = new(); // would this work?
        public static Player turn;

        public static void InitBoard()
        {

            Console.WriteLine("Initializing Board...");

            Spaces = new Space[8][];

            if (Spaces is not null)
            {
                for (int i = C["A"]; i <= C["H"]; i++)
                {
                    Spaces[i] = new Space[8];
                    for (int j = R["1"]; j <= R["8"]; j++)
                    {
                        // populate empty spaces
                        Spaces[i][j] = new Space(i, j, new Piece());
                    }
                }

                // populate white back line
                Spaces[C["A"]][R["1"]].Piece = new WhiteRook();
                Spaces[C["B"]][R["1"]].Piece = new WhiteKnight();
                Spaces[C["C"]][R["1"]].Piece = new WhiteBishop();
                Spaces[C["D"]][R["1"]].Piece = new WhiteQueen();
                Spaces[C["E"]][R["1"]].Piece = new WhiteKing();
                Spaces[C["F"]][R["1"]].Piece = new WhiteBishop();
                Spaces[C["G"]][R["1"]].Piece = new WhiteKnight();
                Spaces[C["H"]][R["1"]].Piece = new WhiteRook();

                // populate white pawns
                Spaces[C["A"]][R["2"]].Piece = new WhitePawn();
                Spaces[C["B"]][R["2"]].Piece = new WhitePawn();
                Spaces[C["C"]][R["2"]].Piece = new WhitePawn();
                Spaces[C["D"]][R["2"]].Piece = new WhitePawn();
                Spaces[C["E"]][R["2"]].Piece = new WhitePawn();
                Spaces[C["F"]][R["2"]].Piece = new WhitePawn();
                Spaces[C["G"]][R["2"]].Piece = new WhitePawn();
                Spaces[C["H"]][R["2"]].Piece = new WhitePawn();

                // populate black pawns
                Spaces[C["A"]][R["7"]].Piece = new BlackPawn();
                Spaces[C["B"]][R["7"]].Piece = new BlackPawn();
                Spaces[C["C"]][R["7"]].Piece = new BlackPawn();
                Spaces[C["D"]][R["7"]].Piece = new BlackPawn();
                Spaces[C["H"]][R["7"]].Piece = new BlackPawn();
                Spaces[C["E"]][R["7"]].Piece = new BlackPawn();
                Spaces[C["F"]][R["7"]].Piece = new BlackPawn();
                Spaces[C["G"]][R["7"]].Piece = new BlackPawn();

                // populate black back line
                Spaces[C["A"]][R["8"]].Piece = new BlackRook();
                Spaces[C["B"]][R["8"]].Piece = new BlackKnight();
                Spaces[C["C"]][R["8"]].Piece = new BlackBishop();
                Spaces[C["D"]][R["8"]].Piece = new BlackQueen();
                Spaces[C["E"]][R["8"]].Piece = new BlackKing();
                Spaces[C["F"]][R["8"]].Piece = new BlackBishop();
                Spaces[C["G"]][R["8"]].Piece = new BlackKnight();
                Spaces[C["H"]][R["8"]].Piece = new BlackRook();

                WhiteKingSpace = Spaces[C["E"]][R["1"]];
                BlackKingSpace = Spaces[C["E"]][R["8"]];

                turn = Player.White;
            }
        }

        public static void PrintBoard()
        {
            Console.Clear();
            if (Spaces is not null)
            {
                for (int j = R["8"]; j >= R["1"]; j--)
                {
                    Console.Write(j + 1);
                    for (int i = C["A"]; i <= C["H"]; i++)
                    {
                        Console.Write(Spaces[i][j].Piece?.Name);
                        if (i == C["H"])
                        {
                            Console.WriteLine();
                        }
                        if (i == C["H"] && j == R["1"])
                        {
                            Console.WriteLine("  A  B  C  D  E  F  G  H");
                        }
                    }
                }
            }
            Console.WriteLine($"-{ChessBoard.turn}'s Turn-");
        }

        public static Space UserSelectsSpace()
        {
            string? selectedPieceColumn = "Z";
            string? selectedPieceRow = "0";
            while (!(Regex.Match(selectedPieceColumn!, "^[A-Ha-h]$").Success))
            {
                Console.WriteLine("Please enter a letter (A-H) or T to TakeBack");
                selectedPieceColumn = Console.ReadLine();

                if (selectedPieceColumn == "T")
                {
                    TakeBackLastMove();
                    PrintBoard();
                }
            }

            while (!(Regex.Match(selectedPieceRow!, "^[1-8]$").Success))
            {
                Console.WriteLine("Please enter a number (1-8)");
                selectedPieceRow = Console.ReadLine();
            }

            //Spaces![C[selectedPieceColumn?.ToUpper()!]]
            //              [R[selectedPieceRow!]].PrintInfo();

            return Spaces![C[selectedPieceColumn?.ToUpper()!]]
                          [R[selectedPieceRow!]];
        }

        public static void ChangeTurn()
        {
            if (turn == Player.White)
            {
                turn = Player.Black;
            }
            else
            {
                turn = Player.White;
            }
        }

        public static void FindAllSpacesAttacked()
        {
            // reset UnderAttack flags
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // reset being attacked flags
                    Spaces![i][j].IsUnderAttackByBlack = false;
                    Spaces![i][j].IsUnderAttackByWhite = false;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Spaces![i][j].Piece?.BelongsTo == Player.White)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            for (int m = 0; m < 8; m++)
                            {
                                if ((Spaces![i][j].Piece!
                                    .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                    !(Spaces![i][j].Piece!
                                    .IsBlocked(Spaces[i][j], Spaces[k][m])))
                                {
                                    Spaces[k][m].IsUnderAttackByWhite = true;
                                }
                            }
                        }
                    }
                    if (Spaces[i][j].Piece!.BelongsTo == Player.Black)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            for (int m = 0; m < 8; m++)
                            {
                                if ((Spaces[i][j].Piece!
                                    .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                    !(Spaces![i][j].Piece!
                                    .IsBlocked(Spaces[i][j], Spaces[k][m])))
                                {
                                    Spaces[k][m].IsUnderAttackByBlack = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void AddMoveToHistory(Space fromSpace, Space toSpace)
        {
            //Piece? startingPiece = fromSpace.Piece;

            //Space? changedSpace = startingPiece?.TryCaptureReturnSpace(fromSpace, toSpace); // store changed space for en passant
            //Piece? changedSpacePiece = changedSpace!.Piece;

            //bool startingPieceHasMoved = startingPiece!.HasMoved;
            //bool changedSpacePieceHasMoved = changedSpacePiece!.HasMoved;

            //ChessBoard.MovesPlayed.Push((fromSpace, toSpace, changedSpace, changedSpacePiece, startingPieceHasMoved, changedSpacePieceHasMoved)!);
        }

        public static void ListMovesPlayed()
        {
            foreach (Move move in MovesPlayed)
            {
                //Console.WriteLine($"Moved from {move.Item2} to  {move.Item1}, capturing {move.Item3}");
            }
        }

        public static void TakeBackLastMove()
        {
            // TODO: Fix this based on new Move class **********
            // *************************************************
            // check for very specific castle moves, make very specific takeback?
            if (MovesPlayed.Count > 0)
            {
                // original space, ending space, changed space
                // changed space's piece, changed piece has moved
                // starting piece has moved
                Move lastMove = MovesPlayed.Pop();

                // if castle - do specific takeback
                if ((lastMove.TargetSpace?.Piece!.GetType() == typeof(BlackKing) ||
                    lastMove.TargetSpace?.Piece!.GetType() == typeof(WhiteKing)) &&
                    (lastMove.InitiatingSpace?.Column == C["E"] && lastMove.InitiatingSpace.Row == R["1"]) &&
                    (lastMove.TargetSpace.Column == C["G"] && lastMove.TargetSpace.Row == R["1"]))
                {
                    Spaces![C["E"]][R["1"]].Piece = Spaces![C["G"]][R["1"]].Piece;
                    Spaces![C["H"]][R["1"]].Piece = Spaces![C["F"]][R["1"]].Piece;
                    Spaces![C["E"]][R["1"]].Piece!.HasMoved = false;
                    Spaces![C["H"]][R["1"]].Piece!.HasMoved = false;
                    Spaces![C["F"]][R["1"]].Clear();
                    Spaces![C["G"]][R["1"]].Clear();
                }
                else if ((lastMove.InitiatingSpace?.Piece!.GetType() == typeof(BlackKing) ||
                    lastMove.TargetSpace?.Piece!.GetType() == typeof(WhiteKing)) && 
                    (lastMove.InitiatingSpace.Column == C["E"] && lastMove.InitiatingSpace.Row == R["1"]) &&
                    (lastMove.InitiatingSpace.Column == C["C"] && lastMove.InitiatingSpace.Row == R["1"]))
                {
                    Spaces![C["E"]][R["1"]].Piece = Spaces![C["C"]][R["1"]].Piece;
                    Spaces![C["A"]][R["1"]].Piece = Spaces![C["D"]][R["1"]].Piece;
                    Spaces![C["E"]][R["1"]].Piece!.HasMoved = false;
                    Spaces![C["A"]][R["1"]].Piece!.HasMoved = false;
                    Spaces![C["C"]][R["1"]].Clear();
                    Spaces![C["D"]][R["1"]].Clear();
                }
                else if ((lastMove.InitiatingSpace?.Piece!.GetType() == typeof(BlackKing) ||
                    lastMove.InitiatingSpace?.Piece!.GetType() == typeof(WhiteKing)) &&
                    (lastMove.InitiatingSpace?.Column == C["E"] && lastMove.InitiatingSpace?.Row == R["8"]) &&
                    (lastMove.InitiatingSpace.Column == C["G"] && lastMove.InitiatingSpace.Row == R["8"]))
                {
                    Spaces![C["E"]][R["8"]].Piece = Spaces![C["G"]][R["8"]].Piece;
                    Spaces![C["H"]][R["8"]].Piece = Spaces![C["F"]][R["8"]].Piece;
                    Spaces![C["E"]][R["8"]].Piece!.HasMoved = false;
                    Spaces![C["H"]][R["8"]].Piece!.HasMoved = false;
                    Spaces![C["F"]][R["8"]].Clear();
                    Spaces![C["G"]][R["8"]].Clear();
                }
                else if ((lastMove.InitiatingSpace?.Piece!.GetType() == typeof(BlackKing) ||
                    lastMove.InitiatingSpace?.Piece!.GetType() == typeof(WhiteKing)) &&
                    (lastMove.InitiatingSpace?.Column == C["E"] && lastMove.InitiatingSpace?.Row == R["8"]) &&
                    (lastMove.InitiatingSpace.Column == C["C"] && lastMove.InitiatingSpace.Row == R["8"]))
                {
                    Spaces![C["E"]][R["8"]].Piece = Spaces![C["C"]][R["8"]].Piece;
                    Spaces![C["A"]][R["8"]].Piece = Spaces![C["D"]][R["8"]].Piece;
                    Spaces![C["E"]][R["8"]].Piece!.HasMoved = false;
                    Spaces![C["A"]][R["8"]].Piece!.HasMoved = false;
                    Spaces![C["C"]][R["8"]].Clear();
                    Spaces![C["D"]][R["8"]].Clear();
                }
                else
                {
                    lastMove.InitiatingSpace!.Piece = lastMove.InitiatingSpace?.Piece;
                    lastMove.InitiatingSpace?.Clear();
                    lastMove.InitiatingSpace!.Piece!.HasMoved = lastMove.InitiatingSpace.Piece.HasMoved;
                    lastMove.InitiatingSpace.Piece = lastMove.InitiatingSpace.Piece;
                }
                ChangeTurn();
            }
        }

        public static bool EitherKingIsInCheck()
        {
            FindAllSpacesAttacked();
            if (turn == Player.White && WhiteKingSpace!.IsUnderAttackByBlack)
            {
                return true;
            }
            else if (turn == Player.Black && BlackKingSpace!.IsUnderAttackByWhite)
            {
                return true;
            }
            return false;
        }

        // if current turn's king is in check
        // if piece can move anywhere that results in the current turn's king not being in check, it is not checkmate

        // if current turn's king is not in check
        // if no piece can move anywhere without putting its own king in check
        public static bool WhiteIsCheckMated()
        {
            FindAllSpacesAttacked();
            if (WhiteKingSpace!.IsUnderAttackByBlack)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Spaces![i][j].Piece?.BelongsTo == Player.White)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                for (int m = 0; m < 8; m++)
                                {
                                    if ((Spaces![i][j].Piece!
                                        .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].Piece!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        if (Spaces[i][j].Piece!.TryMove(Spaces[i][j], Spaces[k][m]))
                                        {
                                            //Console.WriteLine("White is not checkmated!");
                                            return false;
                                        }
                                    }
                                    if ((Spaces![i][j].Piece!
                                        .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].Piece!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        if (Spaces[i][j].Piece!.TryCapture(Spaces[i][j], Spaces[k][m]))
                                        {
                                            //Console.WriteLine("White is not checkmated!");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("WHITE IS CHECKMATED!");
                return true;
            }
            //Console.WriteLine("White is not in check!");
            return false;
        }

        public static bool WhiteIsStaleMated()
        {
            FindAllSpacesAttacked();
            if (!(WhiteKingSpace!.IsUnderAttackByBlack))
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Spaces![i][j].Piece?.BelongsTo == Player.White)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                for (int m = 0; m < 8; m++)
                                {
                                    if ((Spaces![i][j].Piece!
                                        .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].Piece!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        if (Spaces[i][j].Piece!.TryMove(Spaces[i][j], Spaces[k][m]))
                                        {
                                            //Console.WriteLine("White is not stalemated!");
                                            return false;
                                        }
                                    }
                                    if ((Spaces![i][j].Piece!
                                        .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].Piece!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        if (Spaces[i][j].Piece!.TryCapture(Spaces[i][j], Spaces[k][m]))
                                        {
                                            //Console.WriteLine("White is not stalemated!");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("WHITE IS STALEMATED!");
                return true;
            }
            //Console.WriteLine("White is in check!");
            return false;
        }

        public static bool BlackIsCheckMated()
        {
            FindAllSpacesAttacked();
            if (BlackKingSpace!.IsUnderAttackByWhite)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Spaces![i][j].Piece?.BelongsTo == Player.Black)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                for (int m = 0; m < 8; m++)
                                {
                                    if ((Spaces![i][j].Piece!
                                        .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].Piece!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        if (Spaces[i][j].Piece!.TryMove(Spaces[i][j], Spaces[k][m]))
                                        {
                                            //Console.WriteLine("Black is not checkmated!");
                                            return false;
                                        }
                                    }
                                    if ((Spaces![i][j].Piece!
                                        .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].Piece!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        if (Spaces[i][j].Piece!.TryCapture(Spaces[i][j], Spaces[k][m]))
                                        {
                                            //Console.WriteLine("Black is not checkmated!");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("BLACK IS CHECKMATED!");
                return true;
            }
            //Console.WriteLine("Black is not in check!");
            return false;
        }

        public static bool BlackIsStaleMated()
        {
            FindAllSpacesAttacked();
            if (!(BlackKingSpace!.IsUnderAttackByWhite))
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Spaces![i][j].Piece?.BelongsTo == Player.Black)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                for (int m = 0; m < 8; m++)
                                {
                                    if ((Spaces![i][j].Piece!
                                        .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].Piece!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        if (Spaces[i][j].Piece!.TryMove(Spaces[i][j], Spaces[k][m]))
                                        {
                                            //Console.WriteLine("Black is not stalemated!");
                                            return false;
                                        }
                                    }
                                    if ((Spaces![i][j].Piece!
                                        .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].Piece!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        if (Spaces[i][j].Piece!.TryCapture(Spaces[i][j], Spaces[k][m]))
                                        {
                                            //Console.WriteLine("Black is not stalemated!");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("BLACK IS STALEMATED!");
                return true;
            }
            //Console.WriteLine("Black is in check!");
            return false;
        }
    }
}