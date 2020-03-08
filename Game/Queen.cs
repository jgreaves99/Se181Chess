using System;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

namespace ChessSE181.Game
{
    public class Queen: Piece
    {
        public Queen(string color) : base(color)
        {
            
        }

        public override bool CanMove(Board board, int fromX, int fromY, int toX, int toY)
        {
            // var dx = Math.Abs(toX - fromX);
            // var dy = Math.Abs(toY - fromY);
            //
            // // must go diagonal, vertical or horizontal
            // if (dx != dy && dx != 0 && dy != 0)
            //     return false;

            // otherwise just check if path is blocked
            return !IsPathBlocked(board, fromX, fromY, toX, toY);
        }

        private new bool IsPathBlocked(Board board, int fromX, int fromY, int toX, int toY)
        {
            return base.IsPathBlocked(board, toX, toY);
        }
    }
}