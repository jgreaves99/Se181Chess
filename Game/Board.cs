using System.Media;
using Microsoft.AspNetCore.SignalR;

namespace ChessSE181.Game
{
    public class Board
    {
        private readonly Tile[,] _spaces;
        private IClientProxy _white;
        private IClientProxy _black;
        private IClientProxy _currentTurn;

        public Board() 
        {
            _spaces = new Tile[8, 8];
            ResetBoard(); 
        }
        
        public void SetWhite(IClientProxy user)
        {
            _white = user;
        }
        
        public void SetBlack(IClientProxy user)
        {
            _black = user;
        }

        public IClientProxy GetWhite()
        {
            return _white;
        }
        
        public IClientProxy GetBlack()
        {
            return _black;
        }

        public Tile GetSpace(int x, int y) 
        {
            return _spaces[x, y]; 
        }

        public void SetSpace(int x, int y, Piece piece)
        {
            var t = _spaces[x, y];
            t.setPiece(piece);
        }

        public bool IsInitialized()
        {
            if (_white == null || _black == null) return false;
            if (_currentTurn == null)
                _currentTurn = _white;
            return true;
        }

        public IClientProxy GetCurrentTurn()
        {
            return _currentTurn;
        }

        public IClientProxy SwitchTurn()
        {
            _currentTurn = _currentTurn.Equals(_white) ? _black : _white;
            return _currentTurn;
        }

        public void ResetBoard()
        {
            var whiteRook1 = new Rook("white");
            var whiteKnight1 =  new Knight("white");
            var whiteBishop1 = new Bishop("white");
            var whiteRook2 = new Rook("white");
            var whiteKnight2 =  new Knight("white");
            var whiteBishop2 = new Bishop("white");
            var whiteKing  = new King("white");
            var whiteQueen = new Queen("white");
            
            var blackRook1 = new Rook("black");
            var blackKnight1 = new Knight("black");
            var blackBishop1 = new Bishop("black");
            var blackRook2 = new Rook("black");
            var blackKnight2 = new Knight("black");
            var blackBishop2 = new Bishop("black");
            var blackKing = new King("black");
            var blackQueen = new Queen("black");
                
            // white pieces 
            _spaces[0, 0] = new Tile(0, 0, whiteRook1); 
            _spaces[1, 0] = new Tile(1, 0, whiteKnight1); 
            _spaces[2, 0] = new Tile(2, 0, whiteBishop1);
            _spaces[3, 0] = new Tile(3, 0, whiteQueen); 
            _spaces[4, 0] = new Tile(4, 0, whiteKing);
            _spaces[5, 0] = new Tile(5, 0, whiteBishop2); 
            _spaces[6, 0] = new Tile(6, 0, whiteKnight2); 
            _spaces[7, 0] = new Tile(7, 0, whiteRook2);
            
            whiteRook1.setTile(_spaces[0, 0]);
            whiteBishop1.setTile(_spaces[1, 0]);
            whiteKnight1.setTile(_spaces[2, 0]);
            whiteQueen.setTile(_spaces[3, 0]);
            whiteKing.setTile(_spaces[4, 0]);
            whiteBishop2.setTile(_spaces[5, 0]);
            whiteBishop2.setTile(_spaces[6, 0]);
            whiteRook2.setTile(_spaces[7, 0]);
            
            for (var i = 0; i < 8; i++)
            {
                var whitePawn = new Pawn("white");
                _spaces[i, 1] = new Tile(i, 1, whitePawn);
                whitePawn.setTile(_spaces[i, 1]);
            }
            
            // black pieces 
            _spaces[0, 7] = new Tile(0,7, blackRook1); 
            _spaces[1, 7] = new Tile(1,7, blackKnight1); 
            _spaces[2, 7] = new Tile(2,7, blackBishop1);
            _spaces[3, 7] = new Tile(3,7, blackQueen); 
            _spaces[4, 7] = new Tile(4,7, blackKing);
            _spaces[5, 7] = new Tile(5,7, blackBishop2); 
            _spaces[6, 7] = new Tile(6,7, blackKnight2); 
            _spaces[7, 7] = new Tile(7,7, blackRook2);
            
            blackRook1.setTile(_spaces[7, 0]);
            blackKnight1.setTile(_spaces[7, 1]);
            blackBishop1.setTile(_spaces[7, 2]);
            blackQueen.setTile(_spaces[7, 3]);
            blackKing.setTile(_spaces[7, 4]);
            blackBishop2.setTile(_spaces[7, 5]);
            blackKnight2.setTile(_spaces[7, 6]);
            blackRook2.setTile(_spaces[7, 7]);
            
            for (var i = 0; i < 8; i++)
            {
                var blackPawn = new Pawn("black");
                _spaces[i, 6] = new Tile(i, 6, blackPawn);
                blackPawn.setTile(_spaces[i, 6]);
            }


            for (var i = 0; i < 8; i++) { 
                for (var j = 2; j < 6; j++) { 
                    _spaces[i, j] = new Tile(i, j, null); 
                } 
            } 
        } 
    }
}