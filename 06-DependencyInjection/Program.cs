// using DependencyInjection.interfaces;
// using DependencyInjection.Services;

// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddTransient<IWelcomeService, WelcomeService>();
// var app = builder.Build();

// app.MapGet("/", (IWelcomeService welcomeService1, IWelcomeService welcomeService2) =>
// {
//     string message1 = $"Message1: {welcomeService1.GetWelcomeMessage()}";
//     string message2 = $"Message2: {welcomeService2.GetWelcomeMessage()}";
//     return $"{message1}\n{message2}";
// });

// app.Run();
using DependencyInjection.Interfaces;
using DependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services with different lifetimes
builder.Services.AddTransient<IOperationTransient, Operation>();
builder.Services.AddScoped<IOperationScoped, Operation>();
builder.Services.AddSingleton<IOperationSingleton, Operation>();

// Register the service that depends on all three
builder.Services.AddTransient<OperationService>();

var app = builder.Build();

app.MapGet("/", async (HttpContext context,
    IOperationTransient transient1,
    IOperationScoped scoped1,
    IOperationSingleton singleton1,
    OperationService operationService,
    IOperationTransient transient2,
    IOperationScoped scoped2,
    IOperationSingleton singleton2) =>
{
    var response = $@"
Dependency Injection Lifetime Demo
====================================

First Set (Injected directly):
- Transient 1: {transient1.OperationId}
- Scoped 1:    {scoped1.OperationId}
- Singleton 1: {singleton1.OperationId}

Second Set (Injected directly):
- Transient 2: {transient2.OperationId}
- Scoped 2:    {scoped2.OperationId}
- Singleton 2: {singleton2.OperationId}

From OperationService:
- Transient:   {operationService.TransientOperation.OperationId}
- Scoped:      {operationService.ScopedOperation.OperationId}
- Singleton:   {operationService.SingletonOperation.OperationId}

Observations:
=============
TRANSIENT: All 3 instances have DIFFERENT IDs (new instance every time)
SCOPED:    All 3 instances have the SAME ID (same instance per request)
SINGLETON: All 3 instances have the SAME ID (same instance for app lifetime)

Refresh the page to see how Singleton keeps the same ID across requests!
";

    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync(response);
});

app.Run();