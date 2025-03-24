namespace ConsoleChessV3.Interfaces
{
    /// <summary>
    /// Interface for all ChessMoves
    /// </summary>
    internal interface IChessMove
    {
        /// <summary>
        /// Performs the ChessMove
        /// </summary>
        public void Perform();

        /// <summary>
        /// Takes back the ChessMoe
        /// </summary>
        public void Reverse();

        /// <summary>
        /// Validates the ChessMove
        /// </summary>
        /// <returns></returns>
        public bool IsValidChessMove();

        
    }
}
