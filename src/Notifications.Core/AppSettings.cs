namespace Notifications.Core;
public class AppSettings
{
    public DistributedTracingOptions? DistributedTracing { get; set; }
    public DatabaseSettings Database { get; set; } = new();
}

public class DistributedTracingOptions
{
    public bool IsEnabled { get; set; }
    public JaegerOptions? Jaeger { get; set; }
}

public class JaegerOptions
{
    public string? ServiceName { get; set; }
    public string? Host { get; set; }
    public int Port { get; set; }
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "DbConnection";
}