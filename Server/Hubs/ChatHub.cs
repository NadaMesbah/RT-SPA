using Microsoft.AspNetCore.SignalR;

namespace ChatAppSignalR.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();
        public override async Task OnConnectedAsync() 
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            //Context.ConnectionId is the Key
            //username is the value
            Users.Add(Context.ConnectionId, username);
            await AddMessageToChat(string.Empty, $"{username} joined the chat!");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception) 
        { 
            string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await AddMessageToChat(string.Empty, $"{username} left the chat!");
        }
        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("RecieveMessage",user, message);
        }
    }
}
