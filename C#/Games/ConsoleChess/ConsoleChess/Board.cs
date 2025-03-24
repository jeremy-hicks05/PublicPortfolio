

namespace ConsoleChess
{
    using ConsoleChess.Interfaces;
    using ConsoleChess.Pieces;
    using ConsoleChess.Enums;

    using static Enums.Column;
    using static Enums.Row;

    internal static class Board
    {
        // A1 needs to be 0 0
        // A2 needs to be 0 1
        // B2 needs to be 1 1


        public static Space[][] spaces { get; set; } = new Space[8][];
        public static Player turn;
        public static Space? WhiteKingSpace;
        public static Space? BlackKingSpace;

        private static Piece? tempFromSpacePiece;
        private static Piece? tempToSpacePiece;

        public static void InitBoard()
        {
            // Build 2D Spaces Array
            for (Column column = A; column <= H ; column++)
            {
                spaces[(int)column] = new Space[8];
                for (Row row = _8; row <= _1; row++)
                {
                    spaces[(int)column][(int)row] = new Space();
                }
            }

            // Populate empty spaces, declare Letter and Number coordinates for each space
            for (Column column = A; column <= H; column++)
            {
                for (Row row = _8; row <= _1; row++)
                {
                    Space space = new Space();
                    space.Column = (int)column;
                    space.Row = (int)row;
                    space.Piece = new Piece(name: "[ ]", Player.None);
                    spaces[(int)column][(int)row].SetSpace(space);
                }
            }

            // populate Black back row
            spaces[0][0].Piece = new Rook("[r]", Player.Black);
            spaces[0][1].Piece = new Knight("[n]", Player.Black);
            spaces[0][2].Piece = new Bishop("[b]", Player.Black);
            spaces[0][3].Piece = new Queen("[q]", Player.Black);
            spaces[0][4].Piece = new King("[k]", Player.Black);
            spaces[0][5].Piece = new Bishop("[b]", Player.Black);
            spaces[0][6].Piece = new Knight("[n]", Player.Black);
            spaces[0][7].Piece = new Rook("[r]", Player.Black);

            BlackKingSpace = spaces[0][4];

            // populate Black pawns
            for (int i = 0; i < 8; i++)
            {
                spaces[1][i].Piece = new Pawn("[p]", Player.Black);
            }

            // populate White pawns
            for (int i = 0; i < 8; i++)
            {
                spaces[6][i].Piece = new Pawn("[P]", Player.White);
            }

            // populate White back row
            spaces[7][0].Piece = new Rook("[R]", Player.White);
            spaces[7][1].Piece = new Knight("[N]", Player.White);
            spaces[7][2].Piece = new Bishop("[B]", Player.White);
            spaces[7][3].Piece = new Queen("[Q]", Player.White);
            spaces[7][4].Piece = new King("[K]", Player.White);
            spaces[7][5].Piece = new Bishop("[B]", Player.White);
            spaces[7][6].Piece = new Knight("[N]", Player.White);
            spaces[7][7].Piece = new Rook("[R]", Player.White);

            WhiteKingSpace = spaces[7][4];

            FindAllSpacesAttacked();

            PrintBoard();
            turn = Player.White;
        }


        public static Space GetStartingSpace()
        {
            int Number = GetRowForStartingSpace();
            int Letter = GetColumnForStartingSpace();

            return spaces[Letter][Number];
        }

        public static Space GetDestinationSpace()
        {
            int y = GetRowForDestinationSpace();
            int Letter = GetColumnForDestinationSpace();

            return spaces[Letter][y];
        }

        public static int GetColumnForStartingSpace()
        {
            int startLongitude = -1;

            Console.WriteLine();
            while (!(startLongitude >= 0 && startLongitude <= 7))
            {
                Console.Write("Enter Number for " + turn + " Piece to be moved (1-8):");
                startLongitude = NumberToNotation(Console.ReadLine());
                if (!(startLongitude >= 0 && startLongitude <= 7))
                {
                    Console.WriteLine("Please enter a Number 1-8");
                }
            }
            return startLongitude;
        }

        public static int GetRowForStartingSpace()
        {

            int startLatitude = -1;

            while (!(startLatitude >= 0 && startLatitude <= 7))
            {
                Console.Write("Enter Letter for " + turn + " Piece to be moved (A-H):");
                startLatitude = LetterToNotation(Console.ReadLine());
                if (!(startLatitude >= 0 && startLatitude <= 7))
                {
                    Console.WriteLine("Please enter a letter A-H");
                }
            }
            return startLatitude;
        }

        public static int GetColumnForDestinationSpace()
        {

            int endLongitude = -1;

            while (!(endLongitude >= 0 && endLongitude <= 7))
            {
                Console.Write("Enter Number for Space to be moved to (1-8):");
                endLongitude = NumberToNotation(Console.ReadLine());
                if (!(endLongitude >= 0 && endLongitude <= 7))
                {
                    Console.WriteLine("Please enter a number 1-8");
                }
            }
            return endLongitude;
        }

        public static int GetRowForDestinationSpace()
        {

            int endLatitude = -1;
            while (!(endLatitude >= 0 && endLatitude <= 7))
            {
                Console.Write("Enter Letter for Space to be moved to (A-H):");
                endLatitude = LetterToNotation(Console.ReadLine());
                if (!(endLatitude >= 0 && endLatitude <= 7))
                {
                    Console.WriteLine("Please enter a letter A-H");
                }
            }
            return endLatitude;
        }

        public static Space GetSpace(string letter, string number)
        {
            Console.WriteLine("Getting space on ["  +
                NumberToNotation(number) + 
                "][" + LetterToNotation(letter) + "]");
            return spaces[NumberToNotation(number)]
                         [LetterToNotation(letter)];
        }

        public static void ChangeTurns()
        {
            if (turn == Player.Black)
            {
                turn = Player.White;
                Console.WriteLine("It is now White's turn");
            }
            else
            {
                turn = Player.Black;
                Console.WriteLine("It is now Black's turn");
            }
        }

        public static void PrintBoard()
        {
            Console.WriteLine("Press enter to draw board");
            Console.ReadLine();
            Console.Clear();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (j == 0)
                    {
                        Console.Write(8 - i);
                    }
                    Console.Write(spaces[i][j].Piece.Name);
                }

                Console.WriteLine();
            }
            Console.WriteLine("  A  B  C  D  E  F  G  H");
        }

        public static void FindAllSpacesAttacked()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // reset being attacked flags
                    spaces[i][j].IsUnderAttackByBlack = false;
                    spaces[i][j].IsUnderAttackByWhite = false;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (spaces[i][j].Piece.belongsToPlayer == Player.White)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            for (int m = 0; m < 8; m++)
                            {
                                if (spaces[i][j].Piece.CanTryToCapture(spaces[i][j], spaces[k][m]))
                                {
                                    spaces[k][m].IsUnderAttackByWhite = true;
                                    //spaces[k][m].Piece.Name = "[*]";
                                }
                            }
                        }
                    }

                    if (spaces[i][j].Piece.belongsToPlayer == Player.Black)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            for (int m = 0; m < 8; m++)
                            {
                                if (spaces[i][j].Piece.CanTryToCapture(spaces[i][j], spaces[k][m]))
                                {
                                    spaces[k][m].IsUnderAttackByBlack = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void TryMovePieceFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            tempFromSpacePiece = fromSpace.Piece;
            tempToSpacePiece = toSpace.Piece;

            toSpace.Piece = fromSpace.Piece;
            fromSpace.Piece = new Piece("[ ]", Player.None);

            PrintBoard();

            FindAllSpacesAttacked();

            if (toSpace.Piece.GetType() == typeof(King))
            {
                if (turn == Player.Black)
                {
                    BlackKingSpace = toSpace;
                }
                else if (turn == Player.White)
                {
                    WhiteKingSpace = toSpace;
                }
            }

            if (turn == Player.White && WhiteKingIsInCheck())
            {
                // cancel move
                Console.WriteLine("White King is still in check!");
                Console.ReadLine();
                fromSpace.Piece = tempFromSpacePiece;
                toSpace.Piece = tempToSpacePiece;
            }

            else if (turn == Player.Black && BlackKingIsInCheck())
            {
                // cancel move
                Console.WriteLine("Black King is still in check!");
                Console.ReadLine();
                fromSpace.Piece = tempFromSpacePiece;
                toSpace.Piece = tempToSpacePiece;
            }
            else
            {
                // finish successful move
                //fromSpace.Piece = tempFromSpacePiece;
                //toSpace.Piece = tempToSpacePiece;
                //MovePieceFromSpaceToSpace(fromSpace, toSpace);
                ChangeTurns();
            }

            //if (fromSpace.Piece.CanTryToCapture(fromSpace, toSpace))
            //{
            //    // "copy" piece -> it will be in two different locations now
            //    toSpace.Piece = fromSpace.Piece;

            //    // check for unique circumstance wtih regard to king
            //    if (turn == Player.Black && toSpace.Piece.GetType() == typeof(King))
            //    {
            //        BlackKingSpace = toSpace;
            //    }

            //    if (turn == Player.White && toSpace.Piece.GetType() == typeof(King))
            //    {
            //        WhiteKingSpace = toSpace;
            //    }

            //    // store temporary pieces
            //    Piece tempFromSpacePiece = fromSpace.Piece;
            //    Piece tempToSpacePiece = toSpace.Piece;

            //    // try moving the piece (removing piece from starting space)
            //    fromSpace.Piece = new Piece("[ ]", Player.None);

            //    // check spaces attacked with piece temporarily moved
            //    FindAllSpacesAttacked();

            //    // check if own king is in check
            //    if (turn == Player.White && WhiteKingIsInCheck())
            //    {
            //        // cancel move
            //        toSpace.Piece = tempToSpacePiece;
            //        fromSpace.Piece = tempFromSpacePiece;
            //    }
            //    else if (turn == Player.Black && BlackKingIsInCheck())
            //    {
            //        // cancel move
            //        toSpace.Piece = tempToSpacePiece;
            //        fromSpace.Piece = tempFromSpacePiece;
            //    }
            //    else
            //    {
            //        // successful move
            //        toSpace.Piece = tempToSpacePiece;
            //        toSpace.Piece.hasMoved = true;

            //        // check if piece is white pawn on row 8 - pawn promotion
            //        // offer piece selection and transform into selected piece
            //        if (toSpace.Piece.GetType() == typeof(Pawn) &&
            //            toSpace.Piece.belongsToPlayer == Player.White &&
            //            toSpace.Letter == 0)
            //        {
            //            Console.WriteLine("Promotion!");
            //            Console.WriteLine("Select a piece to promote to:");
            //            Console.WriteLine("N: Knight");
            //            Console.WriteLine("B: Bishop");
            //            Console.WriteLine("R: Rook");
            //            Console.WriteLine("Q: Queen");
            //            string? promotionSelection = Console.ReadLine();

            //            switch (promotionSelection)
            //            {
            //                case "N":
            //                    toSpace.Piece = new Knight("[N]", Player.White);
            //                    break;
            //                case "B":
            //                    toSpace.Piece = new Bishop("[B]", Player.White);
            //                    break;
            //                case "R":
            //                    toSpace.Piece = new Rook("[R]", Player.White);
            //                    break;
            //                case "Q":
            //                    toSpace.Piece = new Queen("[Q]", Player.White);
            //                    break;
            //            }
            //        }
            //        // check if piece is black pawn on row 8
            //        // offer piece selection and transform into selected piece
            //        else if (toSpace.Piece.GetType() == typeof(Pawn) &&
            //            toSpace.Piece.belongsToPlayer == Player.Black &&
            //            toSpace.Letter == 7)
            //        {

            //            Console.WriteLine("Promotion!");
            //            Console.WriteLine("Select a piece to promote to:");
            //            Console.WriteLine("N: Knight");
            //            Console.WriteLine("B: Bishop");
            //            Console.WriteLine("R: Rook");
            //            Console.WriteLine("Q: Queen");
            //            string? promotionSelection = Console.ReadLine();

            //            switch (promotionSelection)
            //            {
            //                case "N":
            //                    toSpace.Piece = new Knight("[n]", Player.Black);
            //                    break;
            //                case "B":
            //                    toSpace.Piece = new Bishop("[b]", Player.Black);
            //                    break;
            //                case "R":
            //                    toSpace.Piece = new Rook("[r]", Player.Black);
            //                    break;
            //                case "Q":
            //                    toSpace.Piece = new Queen("[q]", Player.Black);
            //                    break;
            //            }
            //        } // end pawn promotion

            //        // change turns
            //        if (turn == Player.White)
            //        {
            //            turn = Player.Black;
            //        }
            //        else
            //        {
            //            turn = Player.White;
            //        }
            //    }


            //}
            //else // piece cannot attack space, but may still be able to move
            //     // to it?  used for pawns and castling only?
            //{
            //    // check if piece moving to space puts self in check
            //    // "copy" piece -> it will be in two different locations now
            //    toSpace.Piece = fromSpace.Piece;

            //    // check for unique circumstance wtih regard to king
            //    if (turn == Player.Black && toSpace.Piece.GetType() == typeof(King))
            //    {
            //        BlackKingSpace = toSpace;
            //    }

            //    if (turn == Player.White && toSpace.Piece.GetType() == typeof(King))
            //    {
            //        WhiteKingSpace = toSpace;
            //    }

            //    // store temporary piece
            //    Piece tempFromSpacePiece = fromSpace.Piece;

            //    // try moving the piece (removing piece from starting space)
            //    fromSpace.Piece = new Piece("[ ]", Player.None);

            //    // check spaces attacked with piece temporarily moved
            //    FindAllSpacesAttacked();

            //    // check if own king is in check
            //    if (turn == Player.White && WhiteKingIsInCheck())
            //    {
            //        // cancel move
            //        fromSpace.Piece = tempFromSpacePiece;
            //        toSpace.Piece = new Piece("[ ]", Player.None);
            //    }
            //    else if (turn == Player.Black && BlackKingIsInCheck())
            //    {
            //        // cancel move
            //        fromSpace.Piece = tempFromSpacePiece;
            //        toSpace.Piece = new Piece("[ ]", Player.None);
            //    }
            //    else
            //    {
            //        // successful move
            //        toSpace.Piece.hasMoved = true;

            //        // check if piece is white pawn on row 8 - pawn promotion
            //        // offer piece selection and transform into selected piece
            //        if (toSpace.Piece.GetType() == typeof(Pawn) &&
            //            toSpace.Piece.belongsToPlayer == Player.White &&
            //            toSpace.Letter == 0)
            //        {

            //            Console.WriteLine("Promotion!");
            //            Console.WriteLine("Select a piece to promote to:");
            //            Console.WriteLine("N: Knight");
            //            Console.WriteLine("B: Bishop");
            //            Console.WriteLine("R: Rook");
            //            Console.WriteLine("Q: Queen");
            //            string? promotionSelection = Console.ReadLine();

            //            switch (promotionSelection)
            //            {
            //                case "N":
            //                    toSpace.Piece = new Knight("[N]", Player.White);
            //                    break;
            //                case "B":
            //                    toSpace.Piece = new Bishop("[B]", Player.White);
            //                    break;
            //                case "R":
            //                    toSpace.Piece = new Rook("[R]", Player.White);
            //                    break;
            //                case "Q":
            //                    toSpace.Piece = new Queen("[Q]", Player.White);
            //                    break;
            //            }
            //        }
            //        // check if piece is black pawn on row 8
            //        // offer piece selection and transform into selected piece
            //        else if (toSpace.Piece.GetType() == typeof(Pawn) &&
            //            toSpace.Piece.belongsToPlayer == Player.Black &&
            //            toSpace.Letter == 7)
            //        {

            //            Console.WriteLine("Promotion!");
            //            Console.WriteLine("Select a piece to promote to:");
            //            Console.WriteLine("N: Knight");
            //            Console.WriteLine("B: Bishop");
            //            Console.WriteLine("R: Rook");
            //            Console.WriteLine("Q: Queen");
            //            string? promotionSelection = Console.ReadLine();

            //            switch (promotionSelection)
            //            {
            //                case "N":
            //                    toSpace.Piece = new Knight("[n]", Player.Black);
            //                    break;
            //                case "B":
            //                    toSpace.Piece = new Bishop("[b]", Player.Black);
            //                    break;
            //                case "R":
            //                    toSpace.Piece = new Rook("[r]", Player.Black);
            //                    break;
            //                case "Q":
            //                    toSpace.Piece = new Queen("[q]", Player.Black);
            //                    break;
            //            }
            //        } // end pawn promotion

            //        // change turns
            //        if (turn == Player.White)
            //        {
            //            turn = Player.Black;
            //        }
            //        else
            //        {
            //            turn = Player.White;
            //        }
            //    }
            //}
        }

        public static void MovePieceFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            toSpace.Piece = fromSpace.Piece;
            ClearSpace(fromSpace);
            toSpace.Piece.hasMoved = true;

            //FindAllSpacesAttacked();

            ChangeTurns();
        }
        public static int LetterToNotation(string? notation)
        {
            int translatedNotaion = -1;

            if (notation is not null)
            {

                switch (notation.ToUpper())
                {
                    case "A":
                        translatedNotaion = 0;
                        break;
                    case "B":
                        translatedNotaion = 1;
                        break;
                    case "C":
                        translatedNotaion = 2;
                        break;
                    case "D":
                        translatedNotaion = 3;
                        break;
                    case "E":
                        translatedNotaion = 4;
                        break;
                    case "F":
                        translatedNotaion = 5;
                        break;
                    case "2":
                        translatedNotaion = 6;
                        break;
                    case "H":
                        translatedNotaion = 7;
                        break;
                }
            }
            return translatedNotaion;
        }

        public static int NumberToNotation(string? notation)
        {
            int translatedNotaion = -1;

            if (notation is not null)
            {

                switch (notation.ToUpper())
                {
                    case "8":
                        translatedNotaion = 0;
                        break;
                    case "7":
                        translatedNotaion = 1;
                        break;
                    case "6":
                        translatedNotaion = 2;
                        break;
                    case "5":
                        translatedNotaion = 3;
                        break;
                    case "4":
                        translatedNotaion = 4;
                        break;
                    case "3":
                        translatedNotaion = 5;
                        break;
                    case "2":
                        translatedNotaion = 6;
                        break;
                    case "1":
                        translatedNotaion = 7;
                        break;
                }
            }
            return translatedNotaion;
        }

        public static string IntToNotationLetter(int integer)
        {
            string translatedNotaion = "Z";

            switch (integer)
            {
                case 0:
                    translatedNotaion = "H";
                    break;
                case 1:
                    translatedNotaion = "G";
                    break;
                case 2:
                    translatedNotaion = "F";
                    break;
                case 3:
                    translatedNotaion = "E";
                    break;
                case 4:
                    translatedNotaion = "D";
                    break;
                case 5:
                    translatedNotaion = "C";
                    break;
                case 6:
                    translatedNotaion = "B";
                    break;
                case 7:
                    translatedNotaion = "A";
                    break;
            }

            return translatedNotaion;
        }

        public static string IntToNotationNumber(int integer)
        {
            string translatedNotaion = "0";

            switch (integer)
            {
                case 0:
                    translatedNotaion = "8";
                    break;
                case 1:
                    translatedNotaion = "7";
                    break;
                case 2:
                    translatedNotaion = "6";
                    break;
                case 3:
                    translatedNotaion = "5";
                    break;
                case 4:
                    translatedNotaion = "4";
                    break;
                case 5:
                    translatedNotaion = "3";
                    break;
                case 6:
                    translatedNotaion = "2";
                    break;
                case 7:
                    translatedNotaion = "1";
                    break;
            }

            return translatedNotaion;
        }

        public static void CastleKingSideWhite()
        {
            if (spaces[NumberToNotation("1")][LetterToNotation("F")].Piece.belongsToPlayer == Player.None &&
                spaces[NumberToNotation("1")][LetterToNotation("G")].Piece.belongsToPlayer == Player.None)
            {
                Rook myRook = (Rook)spaces[NumberToNotation("1")][LetterToNotation("H")].Piece;
                King myKing = (King)spaces[NumberToNotation("1")][LetterToNotation("E")].Piece;
                if (!myRook.hasMoved && !myKing.hasMoved && !WhiteKingIsInCheck())
                {
                    // check if spaces between the k and r are attacked by black
                    if (spaces[NumberToNotation("1")][LetterToNotation("F")].IsUnderAttackByBlack == false &&
                        spaces[NumberToNotation("1")][LetterToNotation("G")].IsUnderAttackByBlack == false)
                    {
                        spaces[NumberToNotation("1")][LetterToNotation("E")].Piece = new Piece("[ ]", Player.None);
                        spaces[NumberToNotation("1")][LetterToNotation("H")].Piece = new Piece("[ ]", Player.None);

                        spaces[NumberToNotation("1")][LetterToNotation("G")].Piece = new King("[K]", Player.White);
                        spaces[NumberToNotation("1")][LetterToNotation("F")].Piece = new Rook("[R]", Player.White);

                        WhiteKingSpace = spaces[NumberToNotation("1")][LetterToNotation("G")];

                        //ChangeTurns();
                    }
                }
            }
        }

        public static void CastleQueenSideWhite()
        {
            if (spaces[NumberToNotation("1")][LetterToNotation("B")].Piece.belongsToPlayer == Player.None &&
                spaces[NumberToNotation("1")][LetterToNotation("C")].Piece.belongsToPlayer == Player.None &&
                spaces[NumberToNotation("1")][LetterToNotation("D")].Piece.belongsToPlayer == Player.None)
            {
                Rook myRook = (Rook)spaces[NumberToNotation("1")][LetterToNotation("A")].Piece;
                King myKing = (King)spaces[NumberToNotation("1")][LetterToNotation("E")].Piece;
                if (!myRook.hasMoved && !myKing.hasMoved && !WhiteKingIsInCheck())
                {
                    // check if spaces between the k and r are attacked by black
                    if (spaces[NumberToNotation("1")][LetterToNotation("C")].IsUnderAttackByBlack == false &&
                        spaces[NumberToNotation("1")][LetterToNotation("D")].IsUnderAttackByBlack == false)
                    {
                        spaces[NumberToNotation("1")][LetterToNotation("A")].Piece = new Piece("[ ]", Player.None);
                        spaces[NumberToNotation("1")][LetterToNotation("E")].Piece = new Piece("[ ]", Player.None);

                        spaces[NumberToNotation("1")][LetterToNotation("C")].Piece = new King("[K]", Player.Black);
                        spaces[NumberToNotation("1")][LetterToNotation("D")].Piece = new Rook("[R]", Player.Black);

                        WhiteKingSpace = spaces[NumberToNotation("1")][LetterToNotation("C")];

                        //ChangeTurns();
                    }
                }
            }
        }

        public static void CastleKingSideBlack()
        {
            if (spaces[NumberToNotation("8")][LetterToNotation("F")].Piece.belongsToPlayer == Player.None &&
                spaces[NumberToNotation("8")][LetterToNotation("G")].Piece.belongsToPlayer == Player.None)
            {
                Rook myRook = (Rook)spaces[NumberToNotation("8")][LetterToNotation("H")].Piece;
                King myKing = (King)spaces[NumberToNotation("8")][LetterToNotation("E")].Piece;
                if (!myRook.hasMoved && !myKing.hasMoved && !BlackKingIsInCheck())
                {
                    // check if spaces between the k and r are attacked by white
                    if (spaces[NumberToNotation("8")][LetterToNotation("F")].IsUnderAttackByWhite == false &&
                        spaces[NumberToNotation("8")][LetterToNotation("G")].IsUnderAttackByWhite == false)
                    {
                        spaces[NumberToNotation("8")][LetterToNotation("E")].Piece = new Piece("[ ]", Player.None);
                        spaces[NumberToNotation("8")][LetterToNotation("H")].Piece = new Piece("[ ]", Player.None);

                        spaces[NumberToNotation("8")][LetterToNotation("G")].Piece = new King("[k]", Player.Black);
                        spaces[NumberToNotation("8")][LetterToNotation("F")].Piece = new Rook("[r]", Player.Black);

                        BlackKingSpace = spaces[NumberToNotation("8")][LetterToNotation("G")];

                        //ChangeTurns();
                    }
                }
            }
        }

        public static void CastleQueenSideBlack()
        {
            if (spaces[NumberToNotation("8")][LetterToNotation("B")].Piece.belongsToPlayer == Player.None &&
                spaces[NumberToNotation("8")][LetterToNotation("C")].Piece.belongsToPlayer == Player.None &&
                spaces[NumberToNotation("8")][LetterToNotation("D")].Piece.belongsToPlayer == Player.None)
            {
                Rook myRook = (Rook)spaces[NumberToNotation("8")][LetterToNotation("A")].Piece;
                King myKing = (King)spaces[NumberToNotation("8")][LetterToNotation("E")].Piece;
                if (!myRook.hasMoved && !myKing.hasMoved && !BlackKingIsInCheck())
                {
                    // check if spaces between the k and r are attacked by white
                    if (spaces[NumberToNotation("8")][LetterToNotation("C")].IsUnderAttackByWhite == false &&
                        spaces[NumberToNotation("8")][LetterToNotation("D")].IsUnderAttackByWhite == false)
                    {
                        spaces[NumberToNotation("8")][LetterToNotation("E")].Piece = new Piece("[ ]", Player.None);
                        spaces[NumberToNotation("8")][LetterToNotation("A")].Piece = new Piece("[ ]", Player.None);

                        spaces[NumberToNotation("8")][LetterToNotation("C")].Piece = new King("[k]", Player.Black);
                        spaces[NumberToNotation("8")][LetterToNotation("D")].Piece = new Rook("[r]", Player.Black);

                        BlackKingSpace = spaces[NumberToNotation("8")][LetterToNotation("C")];

                        // change turns
                        //ChangeTurns();
                    }
                }
            }
        }

        public static bool WhiteKingIsInCheck()
        {
            // if white king's space is under attack by black
            if (WhiteKingSpace is not null && WhiteKingSpace.IsUnderAttackByBlack)
            {
                //Console.WriteLine("White king in check on space " + WhiteKingSpace.Letter + ", " + WhiteKingSpace.Number);
                return true;
            }
            return false;
        }

        public static bool BlackKingIsInCheck()
        {
            // if black king's space is under attack by white
            if (BlackKingSpace is not null && BlackKingSpace.IsUnderAttackByWhite)
            {
                //Console.WriteLine("Black king in check!");
                return true;
            }
            return false;
        }

        public static void ClearSpace(Space space)
        {
            space.Piece = new Piece("[ ]", Player.None);
        }
    }
}
