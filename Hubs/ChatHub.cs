using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChessSE181.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendChat(string message)
        {
            await Clients.All.SendAsync("ReceiveChat", "Test", message);
        }
    }
}