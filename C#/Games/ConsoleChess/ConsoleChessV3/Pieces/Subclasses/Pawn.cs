namespace ConsoleChessV3.Pieces.Subclasses
{
    internal class Pawn : Piece
    {
        public bool HasJustMovedTwo { get; set; }
        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return fromSpace.GetPiece() is not null &&
                    toSpace.IsEmpty() &&
                    
                    fromSpace.Column == toSpace.Column &&
                        Math.Abs(toSpace.Row - fromSpace.Row) <= 1
                    ||
                    fromSpace.GetPiece() is not null &&
                    !HasMoved &&
                    toSpace.IsEmpty() &&
                    !fromSpace.GetPiece()!.GetHasMoved() &&
                    fromSpace.Column == toSpace.Column &&
                        Math.Abs(toSpace.Row - fromSpace.Row) <= 2;
        }

        public override void BuildListOfSpacesToInspect(Space fromSpace, Space toSpace)
        {
            SpacesToReview.Clear();
            //spacesToCaptureReview?.Clear();
            if (toSpace.Column == fromSpace.Column)
            {
                // moving down
                for (int row = fromSpace.Row - 1; row >= toSpace.Row; row--)
                {
                    SpacesToReview!.Add(ChessBoard.GetSpace(fromSpace.Column, row));
                }
            }
            else if (fromSpace.Column + 1 == toSpace.Column &&
                     fromSpace.Row - 1 == toSpace.Row)
            {
                // attacking down and right
                SpacesToReview!.Add(ChessBoard.GetSpace(toSpace.Column, toSpace.Row));
            }
            else if (fromSpace.Column - 1 == toSpace.Column &&
                     fromSpace.Row - 1 == toSpace.Row)
            {
                // attacking down and left
                SpacesToReview!.Add(ChessBoard.GetSpace(toSpace.Column, toSpace.Row));
            }
        }

        public override void Move(Space fromSpace, Space toSpace)
        {
            HasJustMovedTwo = false;
            if (fromSpace.GetPiece() is not null && toSpace.IsEmpty())
            {
                //fromSpace.GetPiece().SetHasMoved(true);
                if (Math.Abs(toSpace.Row - fromSpace.Row) == 2)
                {
                    HasJustMovedTwo = true;
                }
                HasMoved = true;
                toSpace.SetPiece(fromSpace.GetPiece());
                fromSpace.Clear();
            }
        }
    }
}
