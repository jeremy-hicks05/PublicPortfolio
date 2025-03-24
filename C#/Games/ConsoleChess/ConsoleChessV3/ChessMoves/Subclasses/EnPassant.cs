namespace ConsoleChessV3.ChessMoves.Subclasses
{
    using ConsoleChessV3.ChessMoves;

    internal class EnPassant : ChessMove
    {
        public EnPassant(Space startingSpace, Space endingSpace) : base(startingSpace, endingSpace)
        {
            StartingSpace = startingSpace;
            TargetSpace = endingSpace;

            //CapturedSpace = piece to the left/right of pawn
            RestoreSpace =
                StartingPiece.GetBelongsTo() == Enums.Player.White ?
                ChessBoard.GetSpace(TargetSpace.Column, TargetSpace.Row - 1) : // If WhitePawn
                ChessBoard.GetSpace(TargetSpace.Column, TargetSpace.Row + 1);  // If BlackPawn

            RestorePiece = RestoreSpace?.GetPiece();
        }

        public override void Perform()
        {
            if (StartingPiece.CanLegallyTryToCaptureFromSpaceToSpace(StartingSpace, TargetSpace))
            {
                //TODO: Insert code to perform an EnPassant capture
                TargetSpace.SetPiece(StartingPiece);
                StartingSpace.Clear();
                RestorePiece = RestoreSpace?.GetPiece();
                if (RestoreSpace is not null)
                {
                    RestoreSpace.Clear();
                }
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
        }

        public override bool IsValidChessMove()
        {
            if (StartingSpace is not null && StartingSpace.GetPiece() is not null)
            {
                if (StartingSpace.GetPiece()!.CanLegallyTryToCaptureFromSpaceToSpace(StartingSpace, TargetSpace))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
