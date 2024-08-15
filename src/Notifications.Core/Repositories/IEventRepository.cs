using Notifications.Core.Events;

namespace Notifications.Core.Repositories;

public interface IEventRepository
{
    Task CreateAsync(NotificationEvent notificationEvent);
    Task UpdateStatusAsync(string id, string status, DateTime? sentAt);
    Task<IEnumerable<NotificationEvent>> GetAllAsync();
}