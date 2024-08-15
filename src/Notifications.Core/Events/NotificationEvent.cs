namespace Notifications.Core.Events;

public class NotificationEvent
{
    public string Id { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
}