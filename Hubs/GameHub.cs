using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ChessSE181.Game;
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
                sessionId = (StringValues) "1";
                await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            }

            _getBoard(sessionId);
            
            Console.WriteLine("Session ID: " + sessionId);
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("Register", sessionId);
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
            int fromx, fromy, tox, toy;
            fromx = char.ToUpper(from[0]) - 64 - 1;
            fromy = from[1] - 48 - 1; // extra -1 because array starts at 0 and front end starts at 1
            tox = char.ToUpper(to[0]) - 64 - 1;
            toy = to[1] - 48 - 1;

            if (new[] {fromx, fromy, tox, toy}.Any(i => i < 0 || i > 7))
            {
                await SendMessageToUser(Clients.Caller, "Invalid move: Index out of bounds.");
                return;
            }

            Console.WriteLine("{0}, {1}, {2}, {3}", fromx, fromy, tox, toy);
            
            var tileFrom = board.GetSpace(fromx, fromy);
            var tileTo = board.GetSpace(tox, toy);
            
            string pieceFrom, pieceTo;
            if (tileFrom.getPiece() == null)
            {
                await SendMessageToUser(Clients.Caller, "Invalid move: trying to move a piece that isn't there.");
                return;
            }
            pieceFrom = tileFrom.getPiece().GetType().ToString();
            pieceTo = tileTo.getPiece() == null ? "null" : tileTo.getPiece().GetType().ToString();

            Console.WriteLine("{0}, {1}, {2}", pieceFrom, tileFrom.getX(), tileFrom.getY());
            Console.WriteLine("{0}, {1}, {2}", pieceTo, tileTo.getX(), tileTo.getY());
            
            board.SetSpace(tileTo.getX(), tileTo.getY(), tileFrom.getPiece());
            board.SetSpace(tileFrom.getX(), tileFrom.getY(), null);
            
            tileTo.getPiece().PieceMoved();

            pieceFrom = tileFrom.getPiece() == null ? "null" : tileFrom.getPiece().GetType().ToString();
            pieceTo = tileTo.getPiece() == null ? "null" : tileTo.getPiece().GetType().ToString();

            Console.WriteLine("{0}, {1}, {2}", pieceFrom, tileFrom.getX(), tileFrom.getY());
            Console.WriteLine("{0}, {1}, {2}", pieceTo, tileTo.getX(), tileTo.getY());

            // set turn
            await Clients.Group(sessionId).SendAsync("ReceiveMove", from, to);
        }

        public async Task EndGame(string sessionId, string user)
        {
            
        }
        
        public async Task Surrender(string sessionId, string user)
        {
            await SendChatToGame(sessionId, "System", user + " has surrendered.");
        }

        public async Task EnableTimer(string sessionId, string user)
        {
            await SendChatToGame(sessionId, "System", user + " has requested the timer to be enabled.");
        }
        
        public async Task DisableTimer(string sessionId, string user, string message)
        {
            await SendChatToGame(sessionId, "System", user + " has disabled the timer.");
        }

        public async Task SendMessageToUser(IClientProxy user, string message)
        {
            await user.SendAsync("ReceiveChat", "System", message);
        }
        
        public async Task SendChatToGame(string sessionId, string user, string message)
        {
            await Clients.Group(sessionId).SendAsync("ReceiveChat", user, message);
        }

        private Board _getBoard(string session)
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