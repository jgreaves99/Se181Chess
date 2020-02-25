using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChessSE181.Hubs
{
    public class GameChatHub : Hub
    {
        public async Task SendChat(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveChat", user, message);
        }
    }
}