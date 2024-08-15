using Notifications.WebUI.Models;

namespace Notifications.WebUI.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationEvent>> GetNotificationsAsync();
    Task SendNotificationAsync(NotificationEvent notificationEvent);
}