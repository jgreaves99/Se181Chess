using System;
using System.Reflection.Metadata.Ecma335;

namespace ChessSE181.Game
{
    public class Bishop: Piece
    {
        public Bishop(string color) : base(color)
        {
            
        }

        public override bool CanMove(Board board, int fromX, int fromY, int toX, int toY)
        {
            if(board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }


            // bishops can only move diagonally, dx must equal dy
            if (Math.Abs(toX - fromX) != Math.Abs(toY - fromY)) return false;
            
            // otherwise just check if path is blocked
            return !IsPathBlocked(board, fromX, fromY, toX, toY);
        }
        
        private bool IsPathBlocked(Board board, int fromX, int fromY, int toX, int toY)
        {
            if (base.IsPathBlocked(board, toX, toY)) return true;
            for (var i = 0; i < Math.Abs(toX - fromX); i++)
            {
                if (board.GetSpace(fromX + i, fromY + i).getPiece() != null)
                    return false;
            }
            return false;
        }
    }
}