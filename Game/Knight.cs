using System;

namespace ChessSE181.Game
{
    public class Knight : Piece
    {
        public Knight(string color) : base(color)
        {

        }

        public override bool CanMove(Board board, int fromX, int fromY, int toX, int toY)
        {
            if (IsPathBlocked(board, toX, toY))
                return false;
            
            // if (Math.Abs(toY - fromY) == 2 && Math.Abs(toX - fromX) == 1 || 
            //     Math.Abs(toY - fromY) == 1 && Math.Abs(toX - fromX) == 2) 
            //     return true;
            //
            // return false;

            return true;
        }
    }
}