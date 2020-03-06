using System;
using System.Transactions;

namespace ChessSE181.Game
{
    public class King: Piece
    {
        public King(string color) : base(color)
        {
            
        }

        public override bool CanMove(Board board, int fromX, int fromY, int toX, int toY)
        {
            if (IsPathBlocked(board, toX, toY))
                return false;

            //if for castle
            
            if (Math.Abs(toY - fromY) > 1 || Math.Abs(toX - fromX) > 1) return false;

            return true;
        }
    }
}