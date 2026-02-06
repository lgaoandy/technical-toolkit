using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;
using DependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TenantNotificationSettings>(builder.Configuration);

// Add HttpContextAccessor - required for tenantProvider
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IAudioLogger, AuditLogger>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<ITaskValidator, TaskValidator>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Register notification services with keys
builder.Services.AddKeyedTransient<INotificationService, EmailNotificationService>(NotificationType.Email);
builder.Services.AddKeyedTransient<INotificationService, SmsNotificationService>(NotificationType.SMS);
builder.Services.AddKeyedTransient<INotificationService, PushNotificationService>(NotificationType.Push);

// Register the factory
builder.Services.AddScoped<INotificationServiceFactory, NotificationServiceFactory>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
