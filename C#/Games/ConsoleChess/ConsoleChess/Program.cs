/************************************************
 *                                              *
 * Chess - By Jeremy Hicks (c) 2022             *
 * Tested by - Kari Seitz                       *
 * Early Review(s) - Shaun Lake                 *
 *                                              *
 ************************************************/


namespace ConsoleChess
{
    /* Current Rules Employed:
     * 1. Pawn up 2 on first move
     * 2. King cannot move into check
     * 3. All piece movement restrictions
     * 4. King cannot castle through check
     * 5. King can castle both sides
     * 6. Pieces can capture opponent pieces
     * 7. Pieces cannot move through pieces (eColumncept knight, duh)
     * 8. Inputs can only be A-H and 1-8
     * 9. Implemented changing turns
     * 10. Implemented pawn promotion
     * 11. Pieces can now block check, and cannot move if that piece is pinned
     * 
     */

    /* TODO
     *  1. Implement en passant
     *  2. Force King to not be in check at end of turn
     *  3. Check for stalemate
     *  4. Check for checkmate
     *  5. Prevent pieces from moving if they would put your king in check (may be covered by reverting moves / returning false if king is in check at the end of the turn)
     *      5a. Need to hold 3? temporary values - put the destination piece back if 
     *          the king is in check - if it is a capture, this might capture the piece
     *          and not restore it if I am not careful
     *  6. Refactor, compress, and condense code using functions
     *  7. Allow resignation
     *  8. FiColumn double check bug - taking a piece attacking allows for staying in check
     *  9. FiColumn King duplicating when it escapes check - Move "King Space" as well
     *  10. Change to 'canTryToMoveTo' and 'canActuallyMoveTo'
     *  11. Change to 'canTryToCapture' and 'canActuallyCapture'
    */

    /* Testing
     * 1. Pawns up 2 - on first move only
     * 2. Pawns up 1 - on all others
     * 3. Pawns through pieces - never
     * 4. Pawn capture - only diagonally "forward"
     * 5. Pawn promotion - last row, show menu and promote to N, B, R, or Q
     * 6. Rooks up, down, left, right
     * 7. Rooks through pieces
     * 8. Rook capture
     * 9. Bishop diagonal any direction
     * 10. 
     * 
     */
    internal class Program
    {
        static void Main()
        {
            Board.InitBoard();

            while (true) // White is not in checkmate/stalemate and
                         // Black is not in checkmate/stalemate and
                         // neither has resigned
            {

                Space mySpace = Board.GetSpace("A", "1");

                mySpace.PrintInfo();

                //Space startingSpace = Board.GetStartingSpace();

                //startingSpace.PrintInfo();

                //Space destinationSpace = Board.GetDestinationSpace();

                //destinationSpace.PrintInfo();

                //if (startingSpace.Piece.belongsToPlayer == Board.turn)
                //{
                //    if (startingSpace.Piece.CanTryToMoveFromSpaceToSpace(startingSpace, destinationSpace))
                //    {
                //        Console.WriteLine("Chess rules followed!");
                //        if (startingSpace.Piece.HasPiecesBlockingMoveFromSpaceToSpace(startingSpace, destinationSpace))
                //        {
                //            Console.WriteLine("That piece is blocked!");
                //            Console.ReadLine();
                //        }
                //        else
                //        {
                //            Console.WriteLine("That piece is not blocked!");
                //            Console.ReadLine();

                //            // move piece from startingSpace to destinationSpace
                //            if (startingSpace.Piece.CanCaptureFromSpaceToSpace(startingSpace, destinationSpace))
                //            {
                //                Console.WriteLine("That piece can capture the piece on the destination space!");
                //                Console.ReadLine();
                //                Board.TryMovePieceFromSpaceToSpace(startingSpace, destinationSpace);
                //            }
                //            else
                //            {
                //                Console.WriteLine("That piece can't capture the piece on the destination space!");
                //                Console.ReadLine();
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("It is " + Board.turn + "'s turn!");
                //}

                // check piece's ability to move to selected space
                //Board.MovePieceFromSpaceToSpace(
                //    startingSpace.Piece.CanMoveFromSpaceToSpace(
                //        startingSpace,
                //        destinationSpace),
                //    startingSpace, destinationSpace);

                // refresh spaces' AttackedbyWhite/Black property
                //Board.FindAllSpacesAttacked();

                // re-print board
                Board.PrintBoard();

                // check for stalemate / checkmate
            }
        }
    }
}