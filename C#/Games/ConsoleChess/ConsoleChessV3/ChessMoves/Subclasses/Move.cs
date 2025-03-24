using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChessV3.ChessMoves.Subclasses
{
    internal class Move : ChessMove
    {
        public Move(Space initialSpace, Space targetSpace) : base(initialSpace, targetSpace)
        {

        }

        public override void Perform()
        {
            if (StartingSpace is not null && StartingSpace.GetPiece() is not null)
            {
                StartingSpace.GetPiece()!.Move(StartingSpace, TargetSpace);
            }
        }

        public override void Reverse()
        {
            TargetSpace.Clear();
            StartingSpace.SetPiece(StartingPiece);
            StartingPiece.SetHasMoved(StartingPieceHasMoved);
        }
    }
}
