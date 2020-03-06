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
            var pieceTo = board.GetSpace(toX, toY).getPiece();
            return pieceTo != null && pieceTo.GetColor().Equals(GetColor());
        }
        
        public void PieceMoved()
        {
            if (!_hasMoved)
                _hasMoved = true;
        }

        protected int getIncrementer(int number)
        {
            return 0;
        }
    }
}