namespace ChessSE181.Game
{
    public class Board
    {
        Tile[][] spaces; 
  
        public Board() 
        {
            spaces = new Tile[8][];
            resetBoard(); 
        } 
  
        public Tile getSpace(int x, int y) 
        {
            return spaces[x][y]; 
        }

        public void setSpace(int x, int y, Tile tile)
        {
            spaces[x][y] = tile;
        }

        public void resetBoard() 
        {    
            Rook whiteRook1 = new Rook("white");
            Knight whiteKnight1 =  new Knight("white");
            Bishop whiteBisop1 = new Bishop("white");
            Rook whiteRook2 = new Rook("white");
            Knight whiteKnight2 =  new Knight("white");
            Bishop whiteBisop2 = new Bishop("white");
            King whiteKing  = new King("white");
            Queen whiteQueen = new Queen("white");
            
            Rook blackRook1 = new Rook("black");
            Knight blackKnight1 =  new Knight("black");
            Bishop blackBisop1 = new Bishop("black");
            Rook blackRook2 = new Rook("black");
            Knight blackKnight2 =  new Knight("black");
            Bishop blackBisop2 = new Bishop("black");
            King blackKing = new King("black");
            Queen blackQueen = new Queen("black");
                
            // white pieces 
            spaces[0][0] = new Tile(0, 0, whiteRook1); 
            spaces[0][1] = new Tile(0, 1, whiteKnight1); 
            spaces[0][2] = new Tile(0, 2, whiteBisop1);
            spaces[0][3] = new Tile(0, 3, whiteQueen); 
            spaces[0][4] = new Tile(0, 4, whiteKing);
            spaces[0][5] = new Tile(0, 5, whiteRook2); 
            spaces[0][6] = new Tile(0, 6, whiteKnight2); 
            spaces[0][7] = new Tile(0, 7, whiteBisop2);
            
            whiteRook1.setTile(spaces[0][0]);
            whiteBisop1.setTile(spaces[0][1]);
            whiteKnight1.setTile(spaces[0][2]);
            whiteQueen.setTile(spaces[0][3]);
            whiteKing.setTile(spaces[0][4]);
            whiteRook2.setTile(spaces[0][5]);
            whiteBisop2.setTile(spaces[0][6]);
            whiteKnight2.setTile(spaces[0][7]);
            
            for (int i = 0; i < 7; i++)
            {
                Pawn whitePawn = new Pawn("white");
                spaces[1][i] = new Tile(1, i, whitePawn);
                whitePawn.setTile(spaces[1][i]);
            }
            
            // black pieces 
            spaces[7][0] = new Tile(7, 0,blackRook1); 
            spaces[7][1] = new Tile(7, 1, blackKnight1); 
            spaces[7][2] = new Tile(7, 2, blackBisop1);
            spaces[7][3] = new Tile(7, 3, blackQueen); 
            spaces[7][4] = new Tile(7, 4, blackKing);
            spaces[7][5] = new Tile(7, 5, blackRook2); 
            spaces[7][6] = new Tile(7, 6, blackKnight2); 
            spaces[7][7] = new Tile(7, 7, blackBisop2);
            
            for (int i = 0; i < 7; i++)
            {
                Pawn blackPawn = new Pawn("black");
                spaces[6][i] = new Tile(6, i, blackPawn);
                blackPawn.setTile(spaces[6][i]);
            }


            for (int i = 2; i < 6; i++) { 
                for (int j = 0; j < 8; j++) { 
                    spaces[i][j] = new Tile(i, j, null); 
                } 
            } 
        } 
    }
}