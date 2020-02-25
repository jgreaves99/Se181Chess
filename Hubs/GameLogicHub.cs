using System.Threading.Tasks;
using ChessSE181.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace ChessSE181.Hubs
{
    public class GameLogicHub : Hub
    {
        private readonly GameChatHub _chatHub;

        public GameLogicHub(GameChatHub chatHub)
        {
            _chatHub = chatHub;
        }
        public async Task SendMove(string user, string piece)
        {
            await Clients.All.SendAsync(piece);
        }

        public async Task Surrender(string user)
        {
            await _chatHub.SendChat("System", user + " has surrendered.");
        }
        
        public async Task EnableTimer(string user)
        {
            await _chatHub.SendChat("System", user + " has requested the timer to be enabled.");
        }
        
        public async Task DisableTimer(string user, string message)
        {
            await _chatHub.SendChat("System", user + " has disabled the timer.");
        }
    }
}