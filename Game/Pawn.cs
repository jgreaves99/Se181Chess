using System;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

namespace ChessSE181.Game
{
    public class Pawn: Piece
    {
        public Pawn(string color) : base(color)
        {
            
        }

        public override bool CanMove(Board board, int fromX, int fromY, int toX, int toY)
        {
            if (IsPathBlocked(board, toX, toY))
                return false;

            // var dx = Math.Abs(toX - fromX);
            // var dy = Math.Abs(toY - fromY);
            // var color = GetColor();
            //
            // if (dx > 1 || dy > 2) // pawns never move more than one letter over or two numbers up
            //     return false;
            // if (dy == 2 && // first move by a pawn, 2 square move
            //     dx == 0 &&
            //     !HasMoved() &&
            //     board.GetSpace(toX, fromY + (toY - fromY) / 2).getPiece() == null &&
            //     board.GetSpace(toX, fromY + (toY - fromY)).getPiece() == null &&
            //     (color == Color.Black && toY - fromY == -2 || color == Color.White && toY - fromY == 2)
            //     )
            //     return color == Color.Black && toY - fromY == -2 || color == Color.White && toY - fromY == 2;
            //
            // // check if we are not moving right direction one square vertically
            // if ((color != Color.Black || toY - fromY != -1) && 
            //     (color != Color.White || toY - fromY != 1)) 
            //     return false;
            //
            // // if we are just going vertically, nothing to do other than check if the space is open
            // if (dx != 1) return board.GetSpace(toX, toY).getPiece() == null;
            //
            // // if there is a diagonal path, it is all good
            // if (board.GetSpace(toX, toY).getPiece() != null)
            //     return true;
            //
            // // the last thing is to check for en passant
            // return false;
            // // return board.GetSpace(toX, toY).getPiece() == null;
            //
            // // TODO: implement en passant

            return true;
        }
    }
}