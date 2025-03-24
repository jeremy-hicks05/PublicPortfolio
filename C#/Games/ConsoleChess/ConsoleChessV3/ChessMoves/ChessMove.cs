namespace ConsoleChessV3.ChessMoves
{
    using ConsoleChessV3.Interfaces;

    internal class ChessMove : IChessMove
    {
        public Space StartingSpace;
        public IPiece StartingPiece;
        public bool StartingPieceHasMoved;

        public Space TargetSpace;
        public IPiece? TargetPiece;
        public bool TargetPieceHasMoved;

        public Space? RestoreSpace;
        public IPiece? RestorePiece;
        public bool RestorePieceHasMoved;


        public ChessMove(Space startingSpace, Space endingSpace)
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
        }

        public virtual void Perform()
        {
            // must be overridden by subclasses
            throw new NotImplementedException();
        }

        public virtual void Reverse()
        {
            // must be overridden by subclasses
            throw new NotImplementedException();
        }

        public virtual bool IsValidChessMove()
        {
            if (StartingSpace.GetPiece() is not null)
            {
                if (StartingSpace.GetPiece()!.CanLegallyTryToMoveFromSpaceToSpace(StartingSpace, TargetSpace))
                {
                    if (!StartingSpace.GetPiece()!.IsBlocked(StartingSpace, TargetSpace))
                    {
                        if (StartingSpace.GetPiece()!.TryMove(StartingSpace, TargetSpace))
                        {

                            return true;
                        }
                        else
                        {
                            //Console.WriteLine("King would be in check");
                            //Console.ReadLine();
                            return false;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Piece is blocked");
                        //Console.ReadLine();
                        return false;
                    }
                }
                else if (StartingSpace.GetPiece()!.CanLegallyTryToCaptureFromSpaceToSpace(StartingSpace, TargetSpace))
                {
                    if (!StartingSpace.GetPiece()!.IsBlocked(StartingSpace, TargetSpace))
                    {
                        if (StartingSpace.GetPiece()!.TryCapture(StartingSpace, TargetSpace))
                        {
                            return true;
                        }
                        else
                        {
                            //Console.WriteLine("King would be in check");
                            //Console.ReadLine();
                            return false;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Piece is blocked");
                        //Console.ReadLine();
                        return false;
                    }
                }
                Console.WriteLine($"{StartingSpace!.GetPiece()!.GetType().ToString().Split(".").Last()} does not move like that");
                Console.ReadLine();
                return false;
            }
            Console.WriteLine("Starting space is empty.");
            Console.ReadLine();
            return false;
        }
    }
}
