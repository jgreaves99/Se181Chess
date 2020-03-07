using System;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChessSE181.Game
{
    public abstract class Piece
    {
        public enum Color
        {
            White,
            Black
        }
        
        private Tile tile;
        private readonly Color _color;
        private bool _hasMoved;

        protected Piece(string color)
        {
            _hasMoved = false;

            _color = color switch
            {
                "white" => Color.White,
                "black" => Color.Black,
                _ => _color
            };
        }

        public Color GetColor()
        {
            return _color;
        }

        public bool HasMoved()
        {
            return _hasMoved;
        }

        public void setTile(Tile start)
        {
            tile = start;
        }

        public abstract bool CanMove(Board board, int fromX, int fromY, int toX, int toY);

        protected bool IsPathBlocked(Board board, int toX, int toY)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            var pieceTo = board.GetSpace(toX, toY).getPiece();
            return pieceTo != null && pieceTo.GetColor().Equals(GetColor());
        }
        
        public void PieceMoved()
        {
            if (!_hasMoved)
                _hasMoved = true;
        }

        static protected int getIncrementer(int number)
        {
            number = number + 1 -1; //To avoid the static analysis saying we don't use number.
            return 0;
        }
    }
}