using MassTransit;
using Notifications.Core.Events;
using Notifications.Core.Repositories;

namespace Notifications.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController(
    ILogger<NotificationController> logger,
    IPublishEndpoint publishEndpoint,
    IEventRepository eventRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<NotificationEvent>> GetNotifications()
    {
        return await eventRepository.GetAllAsync();
    }

    [HttpPost("send-notification")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationEvent notificationEvent)
    {
        notificationEvent.Id = Guid.NewGuid().ToString();
        notificationEvent.Status = "Pendente";
        notificationEvent.CreatedAt = DateTime.UtcNow;

        await eventRepository.CreateAsync(notificationEvent);
        await publishEndpoint.Publish(notificationEvent);

        logger.LogInformation($"Notification event published: {notificationEvent.Message}");

        return Ok();
    }
}