using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notifications.Core.Events;

namespace Notifications.Core.Repositories;

public class EventRepository : IEventRepository
{
    private readonly IMongoCollection<NotificationEvent> _events;

    public EventRepository(IOptions<AppSettings> appSettings, IMongoClient mongoClient)
    {
        var settings = appSettings.Value;
        var database = mongoClient.GetDatabase(settings.Database.DatabaseName);
        _events = database.GetCollection<NotificationEvent>("Events");
    }

    public async Task CreateAsync(NotificationEvent notificationEvent)
    {
        await _events.InsertOneAsync(notificationEvent);
    }

    public async Task UpdateStatusAsync(string id, string status, DateTime? sentAt)
    {
        var update = Builders<NotificationEvent>.Update
            .Set(e => e.Status, status)
            .Set(e => e.SentAt, sentAt);
        await _events.UpdateOneAsync(e => e.Id == id, update);
    }

    public async Task<IEnumerable<NotificationEvent>> GetAllAsync()
    {
        return await _events.Find(_ => true).ToListAsync();
    }
}