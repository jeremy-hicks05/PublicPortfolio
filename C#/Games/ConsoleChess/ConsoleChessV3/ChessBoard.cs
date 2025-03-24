using ConsoleChessV3.ChessMoves.Subclasses;

namespace ConsoleChessV3
{
    using ConsoleChessV3.Builders;
    using ConsoleChessV3.Enums;
    using ConsoleChessV3.ChessMoves;
    using ConsoleChessV3.Pieces.Black;
    using ConsoleChessV3.Pieces.Subclasses;
    using ConsoleChessV3.Pieces.White;
    using System.Text.RegularExpressions;
    using static ConsoleChessV3.Enums.Notation;
    using Capture = Capture; // prevent ambiguity between Moves.Capture and RegExp.Capture
    using System.Data.Common;

    internal class ChessBoard
    {
        private static Space[][]? Spaces;
        
        public static Stack<ChessMove?>? MovesPlayed = new();

        public static ChessMove? NextMove;

        public static Space? InitialSpace;
        public static Space? TargetSpace;

        public static Player Turn;

        public static Space? WhiteKingSpace;
        public static Space? BlackKingSpace;

        /// <summary>
        /// Populates ChessBoard with all the pieces, sets first turn as White
        /// </summary>
        public static void InitBoard()
        {
            // build 8x8 Chess Board
            Spaces = new Space[8][];

            for (int i = C["A"]; i <= C["H"]; i++)
            {
                Spaces[i] = new Space[8];
                for (int j = R["1"]; j <= R["8"]; j++)
                {
                    Spaces[i][j] = new();
                    Spaces[i][j].Column = i;
                    Spaces[i][j].Row = j;
                }
            }

            for (int i = C["A"]; i <= C["H"]; i++)
            {
                Spaces[i][R["7"]].SetPiece(new BlackPawn());
            }

            for (int i = C["A"]; i <= C["H"]; i++)
            {
                Spaces[i][R["2"]].SetPiece(new WhitePawn());
            }

            Spaces[C["A"]][R["8"]].SetPiece(new BlackRook());
            Spaces[C["B"]][R["8"]].SetPiece(new BlackKnight());
            Spaces[C["C"]][R["8"]].SetPiece(new BlackBishop());
            Spaces[C["D"]][R["8"]].SetPiece(new BlackQueen());
            Spaces[C["E"]][R["8"]].SetPiece(new BlackKing());
            BlackKingSpace = Spaces[C["E"]][R["8"]];
            Spaces[C["F"]][R["8"]].SetPiece(new BlackBishop());
            Spaces[C["G"]][R["8"]].SetPiece(new BlackKnight());
            Spaces[C["H"]][R["8"]].SetPiece(new BlackRook());

            Spaces[C["A"]][R["1"]].SetPiece(new WhiteRook());
            Spaces[C["B"]][R["1"]].SetPiece(new WhiteKnight());
            Spaces[C["C"]][R["1"]].SetPiece(new WhiteBishop());
            Spaces[C["D"]][R["1"]].SetPiece(new WhiteQueen());
            Spaces[C["E"]][R["1"]].SetPiece(new WhiteKing());
            WhiteKingSpace = Spaces[C["E"]][R["1"]];
            Spaces[C["F"]][R["1"]].SetPiece(new WhiteBishop());
            Spaces[C["G"]][R["1"]].SetPiece(new WhiteKnight());
            Spaces[C["H"]][R["1"]].SetPiece(new WhiteRook());

            Turn = Player.White;

            FindAllSpacesAttacked();
            PrintBoard();
        }

        public static string GetUserRowInput(string column)
        {
            string row = "0";
            
            while (!(Regex.Match(row!, "^[1-8]$").Success))
            {
                // add show first half of move (ex. 'e')
                Console.WriteLine(column.ToLower());
                Console.WriteLine("Please enter a number (1-8)");
                row = Console.ReadLine()!.ToUpper();
                PrintBoard();
            }
            return row;
        }

        public static string GetUserColumnInput()
        {
            string column = "Z";
            while (!(Regex.Match(column!, "^[A-Ha-h]$").Success))
            {
                if (MovesPlayed is not null && MovesPlayed.Count > 0)
                {
                    Console.WriteLine("Please enter a letter (A-H) or T to TakeBack or M to list move history");
                }
                else
                {
                    Console.WriteLine("Please enter a letter (A-H)");
                }

                column = Console.ReadLine()!.ToUpper();

                if (MovesPlayed is not null && MovesPlayed.Count > 0 && column == "T")
                {
                    TakeBackMove();
                }
                else if (MovesPlayed is not null && MovesPlayed.Count > 0 && column == "M")
                {
                    ShowMoveHistory();
                }
                PrintBoard();
            }
            return column;
        }

        /// <summary>
        /// Verifies and gets the Letter (Column) and Row (Number) of initial space
        /// </summary>
        public static void GetInitialSpaceInput()
        {
            PrintBoard();
            // get user input (A-H) and (1-8) for initial space
            string selectedPieceColumn = GetUserColumnInput();
            string selectedPieceRow = GetUserRowInput(selectedPieceColumn);
            
            SetInitialSpaceFromInput(selectedPieceColumn, selectedPieceRow);
        }

        /// <summary>
        /// Verifies and gets the Letter (Column) and Row (Number) of target space
        /// </summary>
        public static void GetTargetSpaceInput()
        {
            string selectedPieceColumn = "Z";
            string selectedPieceRow = "0";
            while (!(Regex.Match(selectedPieceColumn!, "^[A-Ha-h]$").Success))
            {
                PrintBoard();
                if (InitialSpace is not null)
                {
                    Console.WriteLine(InitialSpace.PrintNotation() + "->");
                }
                Console.WriteLine("Please enter a letter (A-H)");
                selectedPieceColumn = Console.ReadLine()!.ToUpper();
            }

            while (!(Regex.Match(selectedPieceRow!, "^[1-8]$").Success))
            {
                PrintBoard();
                if (InitialSpace is not null)
                {
                    Console.Write(InitialSpace.PrintNotation() + "-> ");
                }
                Console.WriteLine(selectedPieceColumn.ToLower());
                Console.WriteLine("Please enter a number (1-8)");
                selectedPieceRow = Console.ReadLine()!.ToUpper();
            }
            SetTargetSpaceFromInput(selectedPieceColumn, selectedPieceRow);
        }

        /// <summary>
        /// Sets InitialSpace property
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public static void SetInitialSpaceFromInput(string column, string row)
        {
            if (Spaces is not null)
            {
                Space selectedSpace = Spaces[C[column]][R[row]];
                if (selectedSpace.IsOccupied() && 
                    (Turn == Player.White && 
                    selectedSpace.GetPiece()?.GetBelongsTo() == Player.White) ||
                    (Turn == Player.Black &&
                    selectedSpace.GetPiece()?.GetBelongsTo() == Player.Black))
                {
                    InitialSpace = Spaces[C[column]][R[row]];
                }
                else
                {
                    Console.WriteLine("Please select a space with one of your pieces on it");
                    Console.ReadLine();
                    Console.Clear();
                    PrintBoard();
                    GetInitialSpaceInput();
                }
            }
        }

        /// <summary>
        /// Sets TargetSpace property
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public static void SetTargetSpaceFromInput(string column, string row)
        {
            if (Spaces is not null)
            {
                TargetSpace = Spaces[C[column]][R[row]];

                if(InitialSpace == TargetSpace)
                {
                    Console.WriteLine("Cannot move to the same space");
                    Console.ReadLine();
                    Console.Clear();
                    PrintBoard();
                    //InitialSpace = null;
                    //GetInitialSpaceInput();
                }
            }
        }

        /// <summary>
        /// Returns the space on the chessboard at column y and row x
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Space GetSpace(int column, int row)
        {
            if (Spaces is not null)
            {
                return Spaces[column][row];
            }
            throw new Exception("error");
        }

        /// <summary>
        /// Changes turn from Black to White or White to Black
        /// </summary>
        public static void ChangeTurn()
        {
            // change from White to Black or Black to White when move has been performed
            if (Turn == Player.White)
            {
                Turn = Player.Black;
            }
            else
            {
                Turn = Player.White;
            }
        }

        /// <summary>
        /// Sets "HasJustMovedTwo" flag to false for your pawns when your turn starts
        /// </summary>
        public static void UpdateHasJustMovedTwo()
        {
            if (Spaces is not null)
            {
                // change all 'has just moved two's for player whose turn it is about to become
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Spaces[i][j].GetPiece() is not null && Spaces[i][j].GetPiece()!.GetBelongsTo() == Turn)
                        {
                            if (Spaces[i][j].GetPiece() is Pawn)
                            {
                                Pawn? tempPawn = Spaces[i][j].GetPiece() as Pawn;
                                tempPawn!.HasJustMovedTwo = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Prints ChessBoard and Pieces in an 8x8 grid to the console
        /// </summary>
        public static void PrintBoard()
        {
            ConsoleColor background = ConsoleColor.Black;
            ConsoleColor darkSquare = ConsoleColor.Red;
            ConsoleColor lightSquare = ConsoleColor.White;
            ConsoleColor blackPiece = ConsoleColor.Red;
            ConsoleColor whitePiece = ConsoleColor.White;
            Console.Clear();
            if (Spaces is not null)
            {
                for (int j = R["8"]; j >= R["1"]; j--)
                {
                    Console.Write((j + 1).ToString());
                    Console.BackgroundColor = background;
                    for (int i = C["A"]; i <= C["H"]; i++)
                    {
                        if (Spaces[i][j].GetPiece() is not null)
                        {
                            if (Spaces[i][j].GetPiece()!.GetBelongsTo() == Player.White)
                            {
                                if ((i + j) % 2 == 0)
                                {
                                    Console.ForegroundColor = darkSquare;
                                }
                                else
                                {
                                    Console.ForegroundColor = lightSquare;
                                }
                                Console.Write("[");
                                Console.ForegroundColor = whitePiece;
                                Console.Write(Spaces[i][j].GetPiece()!.GetName());
                                if ((i + j) % 2 == 0)
                                {
                                    Console.ForegroundColor = darkSquare;
                                }
                                else
                                {
                                    Console.ForegroundColor = lightSquare;
                                }
                                Console.Write("]");
                            }
                            else if (Spaces[i][j].GetPiece()!.GetBelongsTo() == Player.Black)
                            {
                                if ((i + j) % 2 == 0)
                                {
                                    Console.ForegroundColor = darkSquare;
                                }
                                else
                                {
                                    Console.ForegroundColor = lightSquare;
                                }
                                Console.Write("[");
                                Console.ForegroundColor = blackPiece;
                                Console.Write(Spaces[i][j].GetPiece()!.GetName());
                                if ((i + j) % 2 == 0)
                                {
                                    Console.ForegroundColor = darkSquare;
                                }
                                else
                                {
                                    Console.ForegroundColor = lightSquare;
                                }
                                Console.Write("]");
                            }
                        }
                        else
                        {
                            if ((i + j) % 2 == 0)
                            {
                                Console.ForegroundColor = darkSquare;
                                Console.Write(Spaces[i][j]);
                            }
                            else
                            {
                                Console.ForegroundColor = lightSquare;
                                Console.Write(Spaces[i][j]);
                            }
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("  A  B  C  D  E  F  G  H");
                Console.WriteLine($"-{Turn}'s turn-");
            }
        }

        /// <summary>
        /// Checks for null Space inputs, Builds NextMove, validates move, performs move
        /// Changes turn, Updates "HasJustMovedTwo", and finally Prints the Board
        /// </summary>
        public static void PlayMove()
        {
            if (InitialSpace is not null &&
                TargetSpace is not null &&
                InitialSpace.GetPiece() is not null)
            {
                if (InitialSpace.GetPiece()!.GetBelongsTo() == Turn)
                {
                    NextMove = MoveBuilder.Build(InitialSpace, TargetSpace);
                    if (NextMove is not null && NextMove.IsValidChessMove())
                    {
                        NextMove.Perform();
                        SaveMoveToHistory();
                        ChangeTurn();
                        UpdateHasJustMovedTwo();
                        PrintBoard();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Chess Move");
                        Console.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Pushes NextMove property to MoveHistory stack
        /// </summary>
        public static void SaveMoveToHistory()
        {
            if (MovesPlayed is not null)
            {
                MovesPlayed.Push(NextMove);
            }
        }

        /// <summary>
        /// Reverses the move on the top of the MoveHistory stack
        /// </summary>
        public static void TakeBackMove()
        {
            if (MovesPlayed is not null && MovesPlayed.Count > 0)
            {
                ChessMove? lastMove = MovesPlayed.Pop();
                if (lastMove is not null)
                {
                    if (MovesPlayed.Count > 1)
                    {
                        ChessMove? lastLastMove = MovesPlayed.Peek();
                        if (
                            lastLastMove is not null && lastLastMove.StartingPiece is Pawn)
                        {
                            Pawn? tempPawn = lastLastMove.StartingPiece as Pawn;
                            if (tempPawn != null &&
                                Math.Abs(lastLastMove.TargetSpace.Row - lastLastMove.StartingSpace.Row) == 2)
                            {
                                tempPawn.HasJustMovedTwo = true;
                            }
                        }
                    }
                    lastMove.Reverse();
                    ChangeTurn();
                }
            }
            PrintBoard();
        }

        /// <summary>
        /// Prints out the Initial and Target spaces of the move history
        /// </summary>
        public static void ShowMoveHistory()
        {
            if (MovesPlayed is not null)
            {
                int i = 1;
                foreach (ChessMove? m in MovesPlayed.Reverse())
                {
                    if (m is not null)
                    {
                        if (m.StartingPiece.GetBelongsTo() == Player.White)
                        {
                            Console.Write(i + ":");
                            i++;
                        }

                        if (m is Capture || m is EnPassant)
                        {
                            if (m.StartingPiece is Pawn)
                            {
                                Console.Write(" " +
                                    m.StartingSpace.PrintNotation() + "x" +
                                    m.TargetSpace.PrintNotation());
                            }
                            else
                            {
                                Console.Write(" " +
                                    m.StartingPiece.GetName().ToUpper() +
                                    m.StartingSpace.PrintNotation() + "x" +
                                    m.TargetPiece?.GetName().ToUpper() +
                                    m.TargetSpace.PrintNotation());
                            }
                        }
                        else if (m is Move)
                        {
                            if (m.StartingPiece is Pawn)
                            {
                                Console.Write(" " + m.TargetSpace.PrintNotation());
                            }
                            else
                            {
                                Console.Write(" " + m.StartingPiece.GetName().ToUpper());
                                Console.Write(m.TargetSpace.PrintNotation());
                            }
                        }
                        else if (m is Castle)
                        {
                            // if king side castle
                            if (TargetSpace?.Column == C["G"])
                            {
                                Console.Write(" o-o");
                            }
                            else // if queen side castle
                            {
                                Console.Write(" o-o-o");
                            }
                        }
                        else
                        {
                            Console.Write(" " +
                                m.StartingPiece.GetName() +
                                m.StartingSpace.PrintNotation() + " -> " +
                                m.TargetPiece?.GetName() +
                                m.TargetSpace.PrintNotation());
                            //Console.WriteLine("Move Type: " + m.GetType().ToString().Split(".").Last());
                        }
                        if (m.StartingPiece.GetBelongsTo() == Player.Black)
                        {
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No moves have been played");
                    }
                }
            }
            Console.ReadLine();
            //Console.Clear();
        }

        public static void ClearUnderAttackFlags()
        {
            if (Spaces is not null)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        // reset being attacked flags
                        Spaces[i][j].IsUnderAttackByBlack = false;
                        Spaces[i][j].IsUnderAttackByWhite = false;
                    }
                }
            }
        }

        /// <summary>
        /// Iterates through the board, attempting to move every piece to every space,
        /// setting the IsUnderAttackBy{Player} property based on the owner of the piece
        /// </summary>
        public static void FindAllSpacesAttacked()
        {
            if (Spaces is not null)
            {
                // reset UnderAttack flags
                ClearUnderAttackFlags();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Spaces[i][j].HasWhitePieceOnIt())
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                for (int m = 0; m < 8; m++)
                                {
                                    if ((Spaces![i][j].GetPiece()!
                                        .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].GetPiece()!
                                        .IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        Spaces[k][m].IsUnderAttackByWhite = true;
                                    }
                                    if (Spaces[i][j].GetPiece() is not null &&
                                        (Spaces![i][j].GetPiece()!
                                        .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].GetPiece()!
                                        .IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        Spaces[k][m].IsUnderAttackByWhite = true;
                                    }
                                }
                            }
                        }
                        if (Spaces[i][j].HasBlackPieceOnIt())
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                for (int m = 0; m < 8; m++)
                                {
                                    if ((Spaces![i][j].GetPiece()!
                                        .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].GetPiece()!
                                        .IsBlocked(Spaces[i][j], Spaces[k][m])))
                                    {
                                        Spaces[k][m].IsUnderAttackByBlack = true;
                                    }
                                    if ((Spaces[i][j].GetPiece()!
                                        .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                        !(Spaces![i][j].GetPiece()!
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
        }

        /// <summary>
        /// Tests if either Player's KingSpace is attacked by the opponent.
        /// Used to determine if a move just made puts your own king in check
        /// </summary>
        /// <returns></returns>
        public static bool KingIsInCheck()
        {
            FindAllSpacesAttacked();
            if (Turn == Player.White && WhiteKingSpace!.IsUnderAttackByBlack)
            {
                return true;
            }
            else if (Turn == Player.Black && BlackKingSpace!.IsUnderAttackByWhite)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks for White being Checkmated
        /// </summary>
        /// <returns></returns>
        public static bool WhiteIsCheckMated()
        {
            if (Turn == Player.White)
            {
                FindAllSpacesAttacked();
                if (WhiteKingSpace!.IsUnderAttackByBlack)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (Spaces is not null &&
                                Spaces[i][j].HasWhitePieceOnIt())
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    for (int m = 0; m < 8; m++)
                                    {
                                        if ((Spaces![i][j].GetPiece()!
                                            .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                            !(Spaces![i][j].GetPiece()!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                        {
                                            // call TryMove on every possible way the King can move
                                            if (Spaces[i][j].GetPiece()!.TryMove(Spaces[i][j], Spaces[k][m]))
                                            {
                                                //Console.WriteLine("White is not checkmated!");
                                                return false;
                                            }
                                        }
                                        if ((Spaces![i][j].GetPiece()!
                                            .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                            !(Spaces![i][j].GetPiece()!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                        {
                                            // call TryCapture on every possible way the King can capture
                                            if (Spaces[i][j].GetPiece()!.TryCapture(Spaces[i][j], Spaces[k][m]))
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
                    Console.ReadLine();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks for White being Stalemated
        /// </summary>
        /// <returns></returns>
        public static bool WhiteIsStaleMated()
        {
            if (Turn == Player.White)
            {
                FindAllSpacesAttacked();
                if (!(WhiteKingSpace!.IsUnderAttackByBlack))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (Spaces![i][j].HasWhitePieceOnIt())
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    for (int m = 0; m < 8; m++)
                                    {
                                        if ((Spaces![i][j].GetPiece()!
                                            .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                            !(Spaces![i][j].GetPiece()!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                        {
                                            if (Spaces[i][j].GetPiece()!.TryMove(Spaces[i][j], Spaces[k][m]))
                                            {
                                                //Console.WriteLine("White is not stalemated!");
                                                return false;
                                            }
                                        }
                                        if ((Spaces![i][j].GetPiece()!
                                            .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                            !(Spaces![i][j].GetPiece()!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                        {
                                            if (Spaces[i][j].GetPiece()!.TryCapture(Spaces[i][j], Spaces[k][m]))
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
                    Console.ReadLine();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks for Black being Checkmated
        /// </summary>
        /// <returns></returns>
        public static bool BlackIsCheckMated()
        {
            if (Turn == Player.Black)
            {
                FindAllSpacesAttacked();
                if (BlackKingSpace!.IsUnderAttackByWhite)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (Spaces![i][j].HasBlackPieceOnIt())
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    for (int m = 0; m < 8; m++)
                                    {
                                        if (Spaces![i][j].GetPiece()!
                                            .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m]) &&
                                            !(Spaces![i][j].GetPiece()!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                        {
                                            if (Spaces[i][j].GetPiece()!.TryMove(Spaces[i][j], Spaces[k][m]))
                                            {
                                                //Console.WriteLine("Black is not checkmated!");
                                                return false;
                                            }
                                        }
                                        if ((Spaces![i][j].GetPiece()!
                                            .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                            !(Spaces![i][j].GetPiece()!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                        {
                                            if (Spaces[i][j].GetPiece()!.TryCapture(Spaces[i][j], Spaces[k][m]))
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
                    Console.ReadLine();
                    return true;
                }
            }
            //Console.WriteLine("Black is not in check!");
            return false;
        }

        /// <summary>
        /// Checks for Black being Stalekmated
        /// </summary>
        /// <returns></returns>
        public static bool BlackIsStaleMated()
        {
            if (Turn == Player.Black)
            {
                FindAllSpacesAttacked();
                if (!(BlackKingSpace!.IsUnderAttackByWhite))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (Spaces![i][j].HasBlackPieceOnIt())
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    for (int m = 0; m < 8; m++)
                                    {
                                        if ((Spaces![i][j].GetPiece()!
                                            .CanLegallyTryToMoveFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                            !(Spaces![i][j].GetPiece()!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                        {
                                            if (Spaces[i][j].GetPiece()!.TryMove(Spaces[i][j], Spaces[k][m]))
                                            {
                                                //Console.WriteLine("Black is not stalemated!");
                                                return false;
                                            }
                                        }
                                        if ((Spaces![i][j].GetPiece()!
                                            .CanLegallyTryToCaptureFromSpaceToSpace(Spaces[i][j], Spaces[k][m])) &&
                                            !(Spaces![i][j].GetPiece()!.IsBlocked(Spaces[i][j], Spaces[k][m])))
                                        {
                                            if (Spaces[i][j].GetPiece()!.TryCapture(Spaces[i][j], Spaces[k][m]))
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
                    Console.ReadLine();
                    return true;
                }
            }
            return false;
        }
    }
}
