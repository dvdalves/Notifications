using Microsoft.Extensions.DependencyInjection;
using Notifications.Core.Extensions;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Notifications.Core.Extensions;

public static class OpenTelemetryExtension
{
    public static void AddOpenTelemetry(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddOpenTelemetry().WithTracing(telemetry =>
        {
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector()
                .AddService(appSettings?.DistributedTracing?.Jaeger?.ServiceName);

            telemetry.AddSource("MassTransit")
                .AddMassTransitInstrumentation()
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .SetSampler(new AlwaysOnSampler())
                .AddJaegerExporter(jaegerOptions =>
                {
                    jaegerOptions.AgentHost = appSettings?.DistributedTracing?.Jaeger?.Host;
                    jaegerOptions.AgentPort = appSettings?.DistributedTracing?.Jaeger?.Port ?? 0;
                });
        });
    }
}