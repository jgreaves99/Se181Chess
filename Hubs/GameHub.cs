using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChessSE181.Hubs
{
    public class GameHub : Hub
    {
        
        public async Task SendMove(string user, string piece)
        {
            
            // set turn
            await Clients.All.SendAsync(piece);
        }

        public async Task Surrender(string user)
        {
            await SendChat("System", user + " has surrendered.");
        }
        
        public async Task EnableTimer(string user)
        {
            await SendChat("System", user + " has requested the timer to be enabled.");
        }
        
        public async Task DisableTimer(string user, string message)
        {
            await SendChat("System", user + " has disabled the timer.");
        }
        
        public async Task SendChat(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveChat", user, message);
        }
    }
}