using ConsoleChessV2.Pieces;

namespace ConsoleChessV2.Moves
{
    internal class Move
    {
        public Space? InitiatingSpace { get; set; }
        public Piece? InitiatingPiece { get; set; }

        public Space? TargetSpace { get; set; }
        public Piece? TargetPiece { get; set; }

        public Space? AffectedSpace { get; set; }
        public Piece? AffectedPiece { get; set; }

        public Move()
        {

        }

        public Move(Space? initiatingSpace, Piece? initiatingPiece, Space? targetSpace, Piece? targetPiece, Space? affectedSpace, Piece? affectedPiece)
        {
            InitiatingSpace = initiatingSpace;
            InitiatingPiece = initiatingPiece;
            TargetSpace = targetSpace;
            TargetPiece = targetPiece;
            AffectedSpace = affectedSpace;
            AffectedPiece = affectedPiece;
        }
    }
}
