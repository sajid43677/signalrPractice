using Microsoft.AspNetCore.SignalR;

namespace signalr.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }
}
