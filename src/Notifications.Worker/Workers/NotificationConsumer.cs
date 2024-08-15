using MassTransit;
using Microsoft.AspNetCore.SignalR.Client;
using Notifications.Core.Events;
using Notifications.Core.Repositories;

public class NotificationConsumer : IConsumer<NotificationEvent>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<NotificationConsumer> _logger;
    private readonly HubConnection _hubConnection;

    public NotificationConsumer(IEventRepository eventRepository, ILogger<NotificationConsumer> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;

        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5053/notificationHub", options =>
            {
                options.HttpMessageHandlerFactory = _ => httpClientHandler;
            })
            .Build();

        _hubConnection.StartAsync().Wait();
    }

    public async Task Consume(ConsumeContext<NotificationEvent> context)
    {
        var notificationEvent = context.Message;

        _logger.LogInformation($"Receiving notification: {notificationEvent.Message}");

        await Task.Delay(5000);

        await _eventRepository.UpdateStatusAsync(notificationEvent.Id, "Enviado", DateTime.UtcNow);

        _logger.LogInformation($"Notification status updated: {notificationEvent.Message}");
        
        if (_hubConnection.State == HubConnectionState.Connected)
        {
            notificationEvent.Status = "Enviado";
            await _hubConnection.InvokeAsync("SendNotification", notificationEvent);
        }
        else
        {
            _logger.LogError("Failed to send notification: SignalR hub connection is not established.");
        }
    }
}
