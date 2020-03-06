using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChessSE181.Game
{
    public abstract class Piece
    {
        private Tile tile;
        private bool _dead = false;
        private bool _white = false;
        private bool _black = false;
        private bool _HasMoved { get; set; }

        public Piece(string color)
        {
            _HasMoved = false;
            
            switch (color)
            {
                case "white":
                    setWhite(_white);
                    break;
                case "black":
                    setBlack(_black);
                    break;
            }
        }
        
        public bool isWhite() 
        { 
            return this._white == true; 
        }

        public void setTile(Tile start)
        {
            tile = start;
        }
        public void setWhite(bool white) 
        { 
            this._white = white; 
        }
        
        public bool isBlack() 
        { 
            return this._black == true; 
        } 
  
        public void setBlack(bool black) 
        { 
            this._black = black; 
        }
        
        public bool isDead() 
        { 
            return this._dead == true; 
        } 
  
        public void setDead(bool dead) 
        { 
            this._dead = dead; 
        }

        //public abstract bool canMove(Board board, Tile start, Tile end);

        public abstract void move(Board board, Tile start, Tile end);

        public void PieceMoved()
        {
            if (!_HasMoved)
                _HasMoved = true;
        }
    }
}