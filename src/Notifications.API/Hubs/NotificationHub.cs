using Microsoft.AspNetCore.SignalR;
using Notifications.Core.Events;

namespace Notifications.API.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotificationToClient(string connectionId, NotificationEvent notificationEvent)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveNotification", notificationEvent);
        }
    }
}