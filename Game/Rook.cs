using System;

namespace ChessSE181.Game
{
    public class Rook: Piece
    {
        public Rook(string color) : base(color)
        {
            
        }

        public override bool CanMove(Board board, int fromX, int fromY, int toX, int toY)
        {
            // var dx = Math.Abs(toX - fromX);
            // var dy = Math.Abs(toY - fromY);
            //
            // // only dx or dy can be more than 0, rooks can only move one direction
            // if (dx != 0 && dy != 0) return false;
            
            // otherwise just check if path is blocked
            return !IsPathBlocked(board, fromX, fromY, toX, toY);
        }

        private new bool IsPathBlocked(Board board, int fromX, int fromY, int toX, int toY)
        {
            return base.IsPathBlocked(board, toX, toY);
        }
    }
}