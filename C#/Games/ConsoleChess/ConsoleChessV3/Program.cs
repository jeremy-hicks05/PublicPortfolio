/* * * * * * * * * * * * * * * * * * * * * * 
 *       ____________________________       *
 *      |__..~~ Console Chess ~~..__ |      *
 *      |----------------------------|      *
 *      |  by Jeremy Hicks (c) 2023  |      *
 *      [____________________________]      *
 *                                          *
 *      Tested By: Kari Seitz               *
 *      Early Version Review: Shaun Lake    *
 *      Late Version Review: Jim Shaver     *
 *                                          *
 * * * * * * * * * * * * * * * * * * * * * */


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Description:                                                 *
 *      Imitates a Chess board, allowing user to input          *
 *      a letter and number combination to select a space       *
 *      on the board, and another space to move the first       *
 *      space's piece to.                                     *
 *                                                              *
 *      Allows for takebacks and records the history of         *
 *      moves made.                                             *
 *      Allows for viewing list of moves made for the           *
 *      duration of the game.                                   *
 *                                                              *
 *      Checks for checkmate and stalemate.                     *
 *      Follows all the rules of chess.                         *
 *                                                              *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


namespace ConsoleChessV3
{
    using static ChessBoard;
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to Chess!");
            Console.WriteLine("Press enter to begin playing...");
            Console.ReadLine();
            InitBoard();

            while ( !BlackIsCheckMated() &&
                    !WhiteIsCheckMated() &&
                    !BlackIsStaleMated() &&
                    !WhiteIsStaleMated() )
            {
                //PrintBoard();
                GetInitialSpaceInput();
                GetTargetSpaceInput();

                PlayMove();
            }
        }
    }
}