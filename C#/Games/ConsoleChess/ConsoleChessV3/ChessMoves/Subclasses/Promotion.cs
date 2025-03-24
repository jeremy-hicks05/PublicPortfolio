using ConsoleChessV3.Pieces.Black;
using ConsoleChessV3.Pieces.White;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleChessV3.ChessMoves.Subclasses
{
    internal class Promotion : ChessMove
    {
        public Promotion(Space startingSpace, Space endingSpace) : base(startingSpace, endingSpace)
        {

        }

        public override void Perform()
        {
            string? promotionChoice = "Z";
            Console.WriteLine("Promotion!");
            while (!Regex.Match(promotionChoice, "[NBRQ]").Success)
            {
                Console.WriteLine("Which piece would you like to promote to?");
                Console.WriteLine("N: Knight");
                Console.WriteLine("B: Bishop");
                Console.WriteLine("R: Rook");
                Console.WriteLine("Q: Queen");
                //Console.WriteLine("C: Cancel");
                promotionChoice = Console.ReadLine()!.ToUpper();

                if (ChessBoard.Turn == Enums.Player.White)
                {
                    switch (promotionChoice)
                    {
                        case "N":
                            TargetSpace.SetPiece(new WhiteKnight());
                            break;
                        case "B":
                            TargetSpace.SetPiece(new WhiteBishop());
                            break;
                        case "R":
                            TargetSpace.SetPiece(new WhiteRook());
                            break;
                        case "Q":
                            TargetSpace.SetPiece(new WhiteQueen());
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (promotionChoice)
                    {
                        case "N":
                            TargetSpace.SetPiece(new BlackKnight());
                            break;
                        case "B":
                            TargetSpace.SetPiece(new BlackBishop());
                            break;
                        case "R":
                            TargetSpace.SetPiece(new BlackRook());
                            break;
                        case "Q":
                            TargetSpace.SetPiece(new BlackQueen());
                            break;
                        default:
                            break;
                    }
                }
                ChessBoard.PrintBoard();
            }
        }

        public override void Reverse()
        {
            // restore pawn and remove promoted piece
            StartingSpace.SetPiece(StartingPiece);
            StartingPiece.SetHasMoved(StartingPieceHasMoved);

            TargetSpace.SetPiece(TargetPiece);
            if (TargetPiece is not null)
            {
                TargetPiece.SetHasMoved(TargetPieceHasMoved);
            }
        }

        public override bool IsValidChessMove()
        {
            return base.IsValidChessMove();
        }
    }
}
