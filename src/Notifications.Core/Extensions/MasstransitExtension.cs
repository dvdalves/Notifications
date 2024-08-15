using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Notifications.Core.Extensions;

public static class MasstransitExtension
{
    public static void AddMassTransitExtension(this IServiceCollection services, IConfiguration configuration, Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext> configureEndpoints = null)
    {
        services.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("RabbitMq"));

                cfg.UseDelayedMessageScheduler();
                configureEndpoints?.Invoke(cfg, ctx);
                cfg.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromSeconds(5)); });
            });
        });
    }
}