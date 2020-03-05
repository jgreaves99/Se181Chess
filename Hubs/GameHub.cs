using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ChessSE181.Game;
using ChessSE181.Services;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;

namespace ChessSE181.Hubs
{
    public class GameHub : Hub
    {

        private Dictionary<string, Board> _boards;

        public GameHub()
        {
            _boards = new Dictionary<string, Board>();
        }

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
            // Board board = new Board();
            //
            // int fromx, fromy, tox, toy;
            // fromx = from[0];
            // fromy = from[1];
            // tox = to[0];
            // toy = to[1];
            //
            // Tile t = board.getSpace(fromx, fromy);
            // t.setX(tox);
            // t.setY(toy);
            // board.setSpace(fromx, fromy, null);
            // board.setSpace(tox, toy, t);

            // Console.WriteLine();
            // set turn
            await Clients.Group(sessionId).SendAsync("ReceiveMove", from, to);
        }

        public async Task EndGame(string sessionId, string user)
        {
            
        }
        
        public async Task Surrender(string sessionId, string user)
        {
            await SendChat(sessionId, "System", user + " has surrendered.");
        }

        public async Task EnableTimer(string sessionId, string user)
        {
            await SendChat(sessionId, "System", user + " has requested the timer to be enabled.");
        }
        
        public async Task DisableTimer(string sessionId, string user, string message)
        {
            await SendChat(sessionId, "System", user + " has disabled the timer.");
        }
        
        public async Task SendChat(string sessionId, string user, string message)
        {
            await Clients.Group(sessionId).SendAsync("ReceiveChat", user, message);
        }

        private Board _getBoard(string session)
        {
            if (!_boards.ContainsKey(session))
            {
                _boards.Add(session, new Board());
            }

            return _boards[session];
        }
    }
}