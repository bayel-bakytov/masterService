using Microsoft.AspNetCore.SignalR;

namespace MasterServiceBack.Notify;

public class NotificationHub : Hub
{
    private static readonly Dictionary<string, string> UserConnections = new Dictionary<string, string>();

    public void RegisterConnection(string userId)
    {
        var connectionId = Context.ConnectionId;

        if (UserConnections.ContainsKey(userId))
        {
            UserConnections[userId] = connectionId;
        }
        else
        {
            UserConnections.Add(userId, connectionId);
        }
    }

    public void UnregisterConnection(string userId)
    {
        if (UserConnections.ContainsKey(userId))
        {
            UserConnections.Remove(userId);
        }
    }

    public void SendNotificationToUser(string userId, string message)
    {
        if (UserConnections.TryGetValue(userId, out var connectionId))
        {
            Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
        }
    }
}