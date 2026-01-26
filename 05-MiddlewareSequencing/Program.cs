using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Primitives;

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
        await context.Response.WriteAsync("{\"error\": \"path not found\"}");
        return;
    }
        
    await next();
});

/*  Authentication Simulator 
    - Checks for a query parameter "?token=secret123"
*/
app.Use(async (context, next ) =>
{
    const string KEYREQUIRED = "token";
    const string VALREQUIRED = "secret123";

    if (!KeyPairExists(context, KEYREQUIRED, VALREQUIRED))
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\": \"unauthorized\"}");
        return;
    }

    static bool KeyPairExists(HttpContext context, string key, string value)
    {
        IQueryCollection queries = context.Request.Query;

        // Looks for specified key-value pair in query
        foreach (KeyValuePair<string, StringValues> kvp in queries)
        {
            if (kvp.Key == key)
            {
                foreach (string? v in kvp.Value)
                    if (v == value)
                        return true;
            }
        }
        return false;
    }

    // Token successful
    context.Request.Headers.Append("X-User", "AuthenticatedUser");
    await next();
});

/*  Response Modifier
    - Adds a customer header X-Processed to all responses that make it through the pipeline
*/
app.Use(async (context, next) =>
{
    await next();
    context.Response.Headers.Append("X-Processed", "true");
});


/*  Terminal Handler
    - /api/hello returns "hello, authenticated user!"
    - /api/time returns current server time
*/
app.Run(async (context) =>
{
    string path = context.Request.Path;

    if (path == "/api/home")
        await context.Response.WriteAsJsonAsync("{\"message\", \"Hello, authenticated user!\"}"); 
    else if (path == "/api/time")
        await context.Response.WriteAsJsonAsync("{\"message\", \"" + DateTime.Now + "\"}"); 
    else
    {
        // We already know by now that path must start with /api/
        string endpoint = path.Remove(0, "/api/".Length);
        await context.Response.WriteAsJsonAsync("{\"message\", \"API endpoint: " + endpoint + "\"}"); 
    }
});

app.Run();
