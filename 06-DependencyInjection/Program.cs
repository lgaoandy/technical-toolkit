using System.Text.Json.Serialization;
using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;
using DependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TenantNotificationSettings>(builder.Configuration);

// Add HttpContextAccessor - required for tenantProvider
builder.Services.AddHttpContextAccessor();

// Bind configuration section to settings class
builder.Services.Configure<AuditLoggerSettings>(
    builder.Configuration.GetSection("AuditLoggerSettings")
);

builder.Services.AddSingleton<IAuditLogger, AuditLogger>();
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<ITaskValidator, TaskValidator>();

builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<ITaskRepository, CachedTaskRepository>();

// Register notification services with keys
builder.Services.AddKeyedTransient<INotificationService, EmailNotificationService>(NotificationType.Email);
builder.Services.AddKeyedTransient<INotificationService, SmsNotificationService>(NotificationType.SMS);
builder.Services.AddKeyedTransient<INotificationService, PushNotificationService>(NotificationType.Push);

// Register the factory
builder.Services.AddScoped<INotificationServiceFactory, NotificationServiceFactory>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Allow enum conversion case-insensitive and use string names
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.MapControllers();

app.Run();
