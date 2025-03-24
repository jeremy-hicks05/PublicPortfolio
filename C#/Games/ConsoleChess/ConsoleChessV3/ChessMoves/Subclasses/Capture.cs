namespace ConsoleChessV3.ChessMoves.Subclasses
{
    using ConsoleChessV3.ChessMoves;

    /// <summary>
    /// Represents the occurrence of a piece being captured in chess.  
    /// It holds the starting, ending, captured, and otherwise affected pieces
    /// when it comes to a capture.  Is reversed when TakeBack function is called.
    /// </summary>
    internal class Capture : ChessMove
    {
        public Capture(Space startingSpace, Space endingSpace) : base(startingSpace, endingSpace)
        {

            StartingSpace = startingSpace;
            StartingPiece = startingSpace.GetPiece()!;
            StartingPieceHasMoved = StartingPiece.GetHasMoved();

            TargetSpace = endingSpace;
            if (TargetSpace.GetPiece() is not null)
            {
                TargetPiece = endingSpace.GetPiece()!;
                TargetPieceHasMoved = TargetPiece.GetHasMoved();
            }

            RestoreSpace = endingSpace;
            if (RestoreSpace.GetPiece() is not null)
            {
                RestorePiece = endingSpace.GetPiece();
                RestorePieceHasMoved = RestorePiece != null && RestorePiece.GetHasMoved();
            }
        }
        public override void Perform()
        {
            if (StartingSpace is not null && StartingSpace.GetPiece() is not null)
            {
                StartingSpace.GetPiece()!.Capture(StartingSpace, TargetSpace);
            }
        }

        public override void Reverse()
        {
            TargetSpace.Clear();
            if (RestoreSpace is not null)
            {
                RestoreSpace.SetPiece(RestorePiece);
            }
            if (RestoreSpace is not null && RestoreSpace.GetPiece() is not null)
            {
                RestoreSpace.GetPiece()!.SetHasMoved(RestorePieceHasMoved);
            }

            StartingSpace.SetPiece(StartingPiece);
            StartingPiece.SetHasMoved(StartingPieceHasMoved);
        }

        public override bool IsValidChessMove()
        {
            if (StartingSpace is not null && StartingSpace.GetPiece() is not null)
            {
                if (StartingSpace.GetPiece()!.CanLegallyTryToCaptureFromSpaceToSpace(StartingSpace, TargetSpace))
                {
                    if (StartingSpace.GetPiece()!.TryCapture(StartingSpace, TargetSpace))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
