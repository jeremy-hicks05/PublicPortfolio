namespace ConsoleChessV3.Pieces.Subclasses
{
    internal class Knight : Piece
    {
        public override bool CanLegallyTryToMoveFromSpaceToSpace(Space fromSpace, Space toSpace)
        {
            return Math.Abs(fromSpace.Column - toSpace.Column) == 2 &&
                    Math.Abs(fromSpace.Row - toSpace.Row) == 1
                    ||
                    Math.Abs(fromSpace.Column - toSpace.Column) == 1 &&
                    Math.Abs(fromSpace.Row - toSpace.Row) == 2;
        }

        public override void BuildListOfSpacesToInspect(Space fromSpace, Space toSpace)
        {
            SpacesToReview.Clear();
            SpacesToReview.Add(toSpace);
        }
    }
}
