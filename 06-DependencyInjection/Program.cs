using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);

// Add HttpContextAccessor - required for tenantProvider
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IAudioLogger, AuditLogger>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<ITaskValidator, TaskValidator>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddKeyedTransient<INotificationService, NotificationService>(NotificationType.Email);
builder.Services.AddKeyedTransient<INotificationService, NotificationService>(NotificationType.SMS);
builder.Services.AddKeyedTransient<INotificationService, NotificationService>(NotificationType.Push);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
