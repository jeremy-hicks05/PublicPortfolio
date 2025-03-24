/************************************************
 * ConsoleChess V2                              *
 * by Jeremy Hicks (c) 2022                     *
 * Tested By: Kari Seitz                        *
 * Early Review - Shaun Lake                    *
 *                                              *
 *                                              *
 ************************************************/

/* TODO: Add stack of moves for popping to "takeback"
 * Test more games alongside lichess
 * Set HasJustMovedTwo to false for all opponent's pieces at the end of a turn
 * Change move storage and takeback to restore HasJustMovedTwo status
 * 
 * 
 */

namespace ConsoleChessV2
{
    internal class Program
    {
        static void Main()
        {
            // Initialize Board
            ChessBoard.InitBoard();
            while (!(ChessBoard.WhiteIsCheckMated()) &&
                    !(ChessBoard.WhiteIsStaleMated()) &&
                    !(ChessBoard.BlackIsCheckMated()) &&
                    !(ChessBoard.BlackIsStaleMated()))
            {
                ChessBoard.PrintBoard();

                // get Starting and Ending Spaces from User Input
                Space startingSpace = ChessBoard.UserSelectsSpace();
                Space endingSpace = ChessBoard.UserSelectsSpace();

                // Attempt Move
                startingSpace.Piece?.ChessMove(startingSpace, endingSpace);

                ChessBoard.PrintBoard();
            }
        }
    }
}