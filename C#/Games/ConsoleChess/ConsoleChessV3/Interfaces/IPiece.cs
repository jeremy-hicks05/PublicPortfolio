using ConsoleChessV3.Enums;

namespace ConsoleChessV3.Interfaces
{
    internal interface IPiece
    {
        /// <summary>
        /// Ensures a piece can legally try to move to a particular space.  
        /// For instance, knights can only move in a certain way
        /// </summary>
        /// <param name="fromSpace"></param>
        /// <param name="toSpace"></param>
        /// <returns></returns>
        public bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace);

        /// <summary>
        /// Ensures a piece can legally try to move to a particular space.  
        /// For instance, pawns can only capture in a certain way
        /// </summary>
        /// <param name="fromSpace"></param>
        /// <param name="toSpace"></param>
        /// <returns></returns>
        public bool CanLegallyTryToCaptureFromSpaceToSpace(Space fromSpace, Space toSpace);

        /// <summary>
        /// Takes the starting and target spaces, and builds a list of spaces
        /// to the target space for determining if the destination can be reached
        /// without hitting another piece (blocked)
        /// </summary>
        /// <param name="fromSpace"></param>
        /// <param name="toSpace"></param>
        public void BuildListOfSpacesToInspect(Space fromSpace, Space toSpace);

        /// <summary>
        /// Iterates through the list of spaces between the starting and ending spaces
        /// and returns whether the list has any occupied spaces before reaching the end
        /// </summary>
        /// <param name="fromSpace"></param>
        /// <param name="toSpace"></param>
        /// <returns></returns>
        public bool IsBlocked(Space fromSpace, Space toSpace);

        /// <summary>
        /// Attemps the move and checks if your king's space is in danger, returning false
        /// if the piece cannot move without breaking the rule that you cannot put your own
        /// king in check
        /// </summary>
        /// <param name="fromSpace"></param>
        /// <param name="toSpace"></param>
        /// <returns></returns>
        public bool TryMove(Space fromSpace, Space toSpace);

        /// <summary>
        /// Attemps the capture and checks if your king's space is in danger, returning false
        /// if the piece cannot move without breaking the rule that you cannot put your own
        /// king in check
        /// </summary>
        /// <param name="fromSpace"></param>
        /// <param name="toSpace"></param>
        /// <returns></returns>
        public bool TryCapture(Space fromSpace, Space toSpace);

        /// <summary>
        /// Moves the piece from the space fromSpace to the space toSpace
        /// </summary>
        /// <param name="fromSpace"></param>
        /// <param name="toSpace"></param>
        public void Move(Space fromSpace, Space toSpace);

        /// <summary>
        /// Captures the piece on the space toSpace with the piece from fromSpace
        /// </summary>
        /// <param name="fromSpace"></param>
        /// <param name="toSpace"></param>
        public void Capture(Space fromSpace, Space toSpace);


        /// <summary>
        /// Returns the name of the Piece
        /// </summary>
        /// <returns></returns>
        public string GetName();

        /// <summary>
        /// Returns the point value of the Piece
        /// </summary>
        /// <returns></returns>
        public int GetPointValue();

        /// <summary>
        /// Returns the property of the Piece storing whether or not it has moved. 
        /// Used in allowing a pawn to move 2 or a King to castle
        /// </summary>
        /// <returns></returns>
        public bool GetHasMoved();

        /// <summary>
        /// Sets the property HasMoved to true or false
        /// </summary>
        /// <param name="moved"></param>
        public void SetHasMoved(bool moved);

        /// <summary>
        /// Returns the player that owns the Piece
        /// </summary>
        /// <returns></returns>
        public Player GetBelongsTo();
    }
}
