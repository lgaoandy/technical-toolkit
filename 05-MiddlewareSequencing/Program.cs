using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

/*  Request Logger 
    - logs when a request starts and when it completes, logs time taken
*/
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request started: {context.Request.Method} {context.Request.Path}");
    
    // Time request
    var stopwatch = Stopwatch.StartNew();
    await next();
    stopwatch.Stop();
    long milliseconds = stopwatch.ElapsedMilliseconds;    

    Console.WriteLine($"Request completed in {milliseconds} ms");
});

/*  Path Filter 
    - Only allows requests to paths starting with /api/
*/
app.Use(async (context, next) =>
{
    string path = context.Request.Path;
    
    if (!path.StartsWith("/api/"))
    {
        context.Response.StatusCode = 404;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\": \"request path does not start with the required /api/\"}");
        return;
    }
        
    await next();
});

app.Run();
