using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChessSE181.Game;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;

namespace ChessSE181.Hubs
{
    public class GameHub : Hub
    {

        private static readonly Dictionary<string, Board> Boards = new Dictionary<string, Board>();

        public override async Task OnConnectedAsync()
        {
            var sessionId = Context.GetHttpContext().Request.Query["sessionId"];
            
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            }
            catch (ArgumentNullException)
            {
                var random = new Random();
                sessionId = (StringValues) random.Next(2).ToString();
                await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            }

            var board = _getBoard(sessionId);

            Piece.Color color;
            if (board.GetWhite() == null)
            {
                board.SetWhite(Clients.Caller);
                color = Piece.Color.White;
            }
            else if (board.GetBlack() == null)
            {
                board.SetBlack(Clients.Caller);
                color = Piece.Color.Black;
            }
            else
            {
                await SendMessageToUser(Clients.Caller,
                    "This game is already running. If this is an error, press \"Reset Game\" in the menu.");
                await base.OnConnectedAsync();
                await Clients.Caller.SendAsync("Register", sessionId, Piece.Color.White);
                //await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
                return;
            }

            Console.WriteLine("Session ID: " + sessionId);
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("Register", sessionId, color);

            await SendMessageToUser(Clients.Caller, "You are connected.");
            // await SendMessageToUser(Clients.Caller, "Your color is: " + color);
            
            if (board.IsInitialized())
            {
                await SendChatToGame(sessionId, "System", "Game is starting.");
            }
            else
                await SendMessageToUser(Clients.Caller, "Waiting for a second player.");

            Boards[sessionId] = board;

        }
        
        /*
        public override Task OnDisconnected()
        {
            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
        */
        
        public async Task SendMove(string sessionId, string user, string from, string to)
        {
            Console.WriteLine("--NEW SEND MOVE CALL--");
            
            var board = _getBoard(sessionId);

            if (!board.IsInitialized())
                return;

            // if (Clients.Caller != board.GetCurrentTurn())
            // {
            //     Console.WriteLine("{0}, {1}", Clients.Caller, board.GetCurrentTurn());
            //     await SendMessageToUser(Clients.Caller, "Cannot make a move: it is not your turn.");
            //     return;
            // }
            
            var fromX = char.ToUpper(@from[0]) - 64 - 1;
            var fromY = @from[1] - 48 - 1;
            var toX = char.ToUpper(to[0]) - 64 - 1;
            var toY = to[1] - 48 - 1;

            if (new[] {fromX, fromY, toX, toY}.Any(i => i < 0 || i > 7))
            {
                await SendMessageToUser(Clients.Caller, "Invalid move: Index out of bounds.");
                return;
            }

            if (fromX == toX && fromY == toY)
                return;

            Console.WriteLine("{0}, {1}, {2}, {3}", fromX, fromY, toX, toY);
            
            var tileFrom = board.GetSpace(fromX, fromY);
            var tileTo = board.GetSpace(toX, toY);

            if (tileFrom.getPiece() == null)
            {
                await SendMessageToUser(Clients.Caller, "Invalid move: trying to move a piece that isn't there.");
                return;
            }
            var pieceFrom = tileFrom.getPiece().GetType().ToString();
            var pieceTo = tileTo.getPiece() == null ? "null" : tileTo.getPiece().GetType().ToString();

            Console.WriteLine("{0}, {1}, {2}", pieceFrom, tileFrom.getX(), tileFrom.getY());
            Console.WriteLine("{0}, {1}, {2}", pieceTo, tileTo.getX(), tileTo.getY());

            var piece = tileFrom.getPiece();
            if (!piece.CanMove(board, fromX, fromY, toX, toY))
            {
                await SendMessageToUser(Clients.Caller, "Invalid move: cannot move piece there.");
                return;
            }

            var targetedPiece = board.GetSpace(toX, toY).getPiece();
            var killedKing = targetedPiece != null && targetedPiece.GetType() == typeof(King);

            board.SetSpace(tileTo.getX(), tileTo.getY(), tileFrom.getPiece());
            board.SetSpace(tileFrom.getX(), tileFrom.getY(), null);
            
            tileTo.getPiece().PieceMoved();

            pieceFrom = tileFrom.getPiece() == null ? "null" : tileFrom.getPiece().GetType().ToString();
            pieceTo = tileTo.getPiece() == null ? "null" : tileTo.getPiece().GetType().ToString();

            Console.WriteLine("{0}, {1}, {2}", pieceFrom, tileFrom.getX(), tileFrom.getY());
            Console.WriteLine("{0}, {1}, {2}", pieceTo, tileTo.getX(), tileTo.getY());

            // set turn
            await Clients.Group(sessionId).SendAsync("ReceiveMove", from, to);
            if (killedKing)
            {
                var color = targetedPiece.GetColor();
                var winner = color switch
                {
                    Piece.Color.White => "Black",
                    Piece.Color.Black => "White",
                    _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
                };

                await SendChatToGame(sessionId, "System", user + "'s king has been killed.");
                await SendChatToGame(sessionId, "System",winner + " player wins!");
                await EndGame(sessionId);
                return;
            }
            
            Boards[sessionId] = board;
        }

        public async Task EndGame(string sessionId)
        {
            await SendChatToGame(sessionId, "System","Refresh the page to start a new game.");
            Boards.Remove(sessionId);
        }
        
        public async Task DrawGame(string sessionId)
        {
            await SendChatToGame(sessionId, "System","Game drawn.");
            await EndGame(sessionId);
        }
        
        public async Task ResetGame(string sessionId)
        {
            await SendChatToGame(sessionId, "System","Game reset.");
            await EndGame(sessionId);
        }
        
        public async Task Surrender(string sessionId, string user)
        {
            var board = _getBoard(sessionId);

            if (!board.IsInitialized())
                return;
            
            await SendChatToGame(sessionId, "System", user + " has surrendered.");
            Piece.Color color;
            var client = Clients.Caller;
            
            if (client.Equals(board.GetWhite()))
                color = Piece.Color.White;
            else if (client.Equals(board.GetBlack()))
                color = Piece.Color.Black;
            else
                color = Piece.Color.White;
            
            await EndGame(sessionId);
        }

        public async Task EnableTimer(string sessionId, string user)
        {
            await SendChatToGame(sessionId, "System", user + " has requested the timer to be enabled.");
        }
        
        public async Task DisableTimer(string sessionId, string user, string message)
        {
            await SendChatToGame(sessionId, "System", user + " has disabled the timer.");
        }

        private static async Task SendMessageToUser(IClientProxy user, string message)
        {
            await user.SendAsync("ReceiveChat", "System", message);
        }
        
        public async Task SendChatToGame(string sessionId, string user, string message)
        {
            var board = _getBoard(sessionId);

            if (!board.IsInitialized())
                return;
            
            await Clients.Group(sessionId).SendAsync("ReceiveChat", user, message);
        }

        private static Board _getBoard(string session)
        {
            if (!Boards.ContainsKey(session))
            {
                Console.WriteLine("this is a new board");
                Boards.Add(session, new Board());
            }

            return Boards[session];
        }
    }
}