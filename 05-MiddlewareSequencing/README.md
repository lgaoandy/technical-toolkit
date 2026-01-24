# ASP.NET Core Middleware Pipeline Challenge

A hands-on exercise to master middleware concepts in ASP.NET Core, focusing on request pipeline flow, terminal vs inline middleware, and execution order.

## 🎯 Learning Objectives

- Understand the difference between `.Use()` and `.Run()`
- Master middleware execution order and pipeline flow
- Learn when to call `next()` and when to short-circuit
- Practice request/response manipulation
- Implement authentication and logging patterns

## 📋 Challenge Requirements

Build a request processing pipeline with the following middleware components (must be in this exact order):

### 1. Request Logger (`.Use()`)

Logs when a request starts and when it completes, including total processing time.

**Expected behavior:**

- Before pipeline: `"Request started: {method} {path}"`
- After pipeline: `"Request completed in {milliseconds}ms"`

### 2. Path Filter (`.Use()`)

Only allows requests to paths starting with `/api/`

**Expected behavior:**

- Invalid path → Stop pipeline, return 404 with message "Path not found"
- Valid path → Continue to next middleware

### 3. Authentication Simulator (`.Use()`)

Checks for query parameter `?token=secret123`

**Expected behavior:**

- Missing/wrong token → Stop pipeline, return 401 with message "Unauthorized"
- Correct token → Add header `X-User: AuthenticatedUser`, continue pipeline

### 4. Response Modifier (`.Use()`)

Adds custom header to all successful responses

**Expected behavior:**

- Add header `X-Processed: true` to all responses that complete the pipeline
- Must execute AFTER the terminal middleware

### 5. Terminal Handler (`.Run()`)

Returns different responses based on path

**Expected behavior:**

- `/api/hello` → "Hello, authenticated user!"
- `/api/time` → Current server time
- Any other `/api/*` → "API endpoint: {path}"

## ✅ Test Cases

| Request                          | Expected Response            | Status | Headers                 |
| -------------------------------- | ---------------------------- | ------ | ----------------------- |
| `GET /home`                      | "Path not found"             | 404    | -                       |
| `GET /api/hello`                 | "Unauthorized"               | 401    | -                       |
| `GET /api/hello?token=wrong`     | "Unauthorized"               | 401    | -                       |
| `GET /api/hello?token=secret123` | "Hello, authenticated user!" | 200    | `X-User`, `X-Processed` |
| `GET /api/time?token=secret123`  | Current server time          | 200    | `X-User`, `X-Processed` |
| `GET /api/users?token=secret123` | "API endpoint: /api/users"   | 200    | `X-User`, `X-Processed` |

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 or later
- Basic understanding of ASP.NET Core

### Running the Project

```bash
# Build the project
dotnet build

# Run the project
dotnet run

# Test with curl or browser
curl http://localhost:5000/api/hello?token=secret123
```

## 🎓 Key Concepts to Understand

### Middleware Execution Flow

```
Request →  Logger (before)
       →  Path Filter
       →  Authentication
       →  Response Modifier (before)
       →  Terminal Handler
       ←  Response Modifier (after)
       ←  Logger (after)  → Response
```

### `.Use()` vs `.Run()`

- **`.Use()`**: Inline middleware that can call `next()` to continue the pipeline
- **`.Run()`**: Terminal middleware that ends the pipeline

### Short-Circuiting

When middleware doesn't call `next()`, it short-circuits the pipeline and starts returning through previous middleware.

## 🏆 Bonus Challenges

Once you complete the basic requirements, try these:

1. **Status Code Logging**: Make the Request Logger show the response status code
2. **Exception Handling**: Add middleware that catches exceptions and returns 500 responses
3. **Teapot Easter Egg**: Add middleware that returns 418 "I'm a teapot" for `/api/teapot`
4. **Rate Limiting**: Add middleware that allows max 5 requests per minute
5. **Request ID**: Add middleware that generates and logs a unique ID for each request

## 📝 Success Criteria

- [ ] All middleware executes in correct order
- [ ] Request Logger shows both start and completion
- [ ] Path Filter properly blocks non-API paths
- [ ] Authentication works with correct/incorrect tokens
- [ ] Response headers are added properly
- [ ] Terminal handler returns correct responses
- [ ] You can explain the execution flow

## 🔍 Debugging Tips

- Use `Console.WriteLine()` liberally to trace execution flow
- Pay attention to whether `next()` is called
- Check middleware order carefully
- Use browser DevTools or curl to inspect headers
- Remember: `.Run()` should be last!

## 📚 Additional Resources

- [ASP.NET Core Middleware Documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/)
- [Middleware Pipeline](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/#middleware-order)

## 🤝 Contributing

This is a learning exercise, but feel free to:

- Add more test cases
- Implement bonus challenges
- Share your solution approach
- Suggest improvements

## 📄 License

This project is for educational purposes.

---

**Happy Learning! 🎉**

Remember: Understanding middleware is crucial for building robust ASP.NET Core applications!
