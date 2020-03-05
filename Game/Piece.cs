using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChessSE181.Game
{
    public abstract class Piece
    {
        private Tile tile;
        private bool dead = false;
        private bool white = false;
        private bool black = false;

        public Piece(string color)
        {
            if (color == "white")
            {
                this.setWhite(white);
            }
            if (color == "black")
            {
                this.setBlack(black);
            }
        }
        
        public bool isWhite() 
        { 
            return this.white == true; 
        }

        public void setTile(Tile start)
        {
            tile = start;
        }
        public void setWhite(bool white) 
        { 
            this.white = white; 
        }
        
        public bool isBlack() 
        { 
            return this.black == true; 
        } 
  
        public void setBlack(bool black) 
        { 
            this.black = black; 
        }
        
        public bool isDead() 
        { 
            return this.dead == true; 
        } 
  
        public void setDead(bool dead) 
        { 
            this.dead = dead; 
        }

        public virtual bool canMove(Board board, Tile start, Tile end)
        {
            start.setPiece(null);
            end.setPiece(this);
            tile = end;
            return true;
        }
    }
}