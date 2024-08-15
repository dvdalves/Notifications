using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notifications.API.Hubs;
using Notifications.Core;
using Notifications.Core.Extensions;
using Notifications.Core.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.AddSerilog("API MassTransit");
Log.Information("Starting API");

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<AppSettings>>().Value;
    return new MongoClient(settings.Database.ConnectionString);
});

builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", null);
});

builder.Services.AddMassTransitExtension(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7224")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample.Masstransit.WebApi v1"));
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.MapHub<NotificationHub>("/notificationHub");

await app.RunAsync();
