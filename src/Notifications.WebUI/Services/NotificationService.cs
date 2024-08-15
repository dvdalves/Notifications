using Notifications.WebUI.Models;

namespace Notifications.WebUI.Services;

public class NotificationService(HttpClient httpClient) : INotificationService
{
    public async Task<IEnumerable<NotificationEvent>> GetNotificationsAsync()
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<NotificationEvent>>("https://localhost:5053/Notification");
    }

    public async Task SendNotificationAsync(NotificationEvent notificationEvent)
    {
        await httpClient.PostAsJsonAsync("https://localhost:5053/Notification/send-notification", notificationEvent);
    }
}