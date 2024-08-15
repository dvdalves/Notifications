using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notifications.Core;
using Notifications.Core.Extensions;
using Notifications.Core.Repositories;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddSerilog("Worker MassTransit");
    Log.Information("Starting Worker");

    builder.Services.Configure<AppSettings>(
        builder.Configuration.GetSection("AppSettings"));

    builder.Services.AddSingleton<IMongoClient>(sp =>
    {
        var settings = sp.GetRequiredService<IOptions<AppSettings>>().Value;
        return new MongoClient(settings.Database.ConnectionString);
    });

    builder.Services.AddScoped<IEventRepository, EventRepository>();

    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<NotificationConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
            cfg.ReceiveEndpoint("notification_queue", e =>
            {
                e.ConfigureConsumer<NotificationConsumer>(context);
            });
        });
    });

    var app = builder.Build();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}