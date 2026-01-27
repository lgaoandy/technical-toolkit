# Dependency Injection Master Challenge: Multi-Tenant Task Management System

## Scenario
You're building a multi-tenant task management API where different organizations share the same application but have isolated data and configurations. You'll implement various service lifetimes, explore advanced DI features, and handle complex dependency scenarios.

## Requirements

### 1. Core Services & Interfaces (30 points)

Create the following services with appropriate interfaces:

- **`ITenantProvider`** - Identifies which tenant (organization) the current request belongs to
- **`ITaskRepository`** - Manages tasks for the current tenant (must be tenant-aware)
- **`INotificationService`** - Sends notifications when tasks are created/updated
- **`IAuditLogger`** - Logs all operations with timestamps and tenant information
- **`ICacheService`** - Provides simple in-memory caching capabilities
- **`ITaskValidator`** - Validates task data before saving

### 2. Service Lifetime Scenarios (40 points)

Implement the services with the following **specific** lifetime requirements and behaviors:

- **Singleton**: `IAuditLogger` should track the total number of operations across ALL requests and ALL tenants since application startup
- **Scoped**: `ITenantProvider` should be determined once per request and remain consistent throughout that request
- **Scoped**: `ITaskRepository` must use the tenant from `ITenantProvider` and only work with that tenant's data
- **Transient**: `INotificationService` should generate a unique notification ID each time it's injected
- **Your choice**: `ICacheService` and `ITaskValidator` - you decide the appropriate lifetime and justify it in comments

### 3. Advanced Challenges (30 points)

**A. Keyed Services**  
Implement multiple notification strategies (EmailNotification, SmsNotification, PushNotification) and use a mechanism to resolve the correct one based on tenant preferences. Research how to register and resolve multiple implementations.

**B. Factory Pattern**  
Create an `ITaskProcessorFactory` that can create different task processors based on task type (e.g., "urgent", "scheduled", "recurring"). The factory should properly resolve dependencies for each processor type.

**C. Service Decoration/Wrapping**  
Implement a caching layer for `ITaskRepository` that wraps the real repository and caches results. The cache should be tenant-aware and use `ICacheService`.

**D. Configuration-based Registration**  
At least one service should change behavior based on configuration (use `IConfiguration` or `IOptions<T>`). For example, `IAuditLogger` might have different log levels or formats based on appsettings.json.

## Technical Constraints

- ✅ Use in-memory collections (List, Dictionary, etc.) for data storage
- ✅ Simulate tenant context (you can use a hardcoded tenant ID or randomize it per request)
- ✅ Include at least 3 API endpoints that demonstrate your DI setup
- ❌ No real databases, file systems, or external services
- ❌ No Entity Framework or ORM libraries
- ✅ Must be a working ASP.NET Core Web API project
- ✅ Include XML comments or regular comments explaining your lifetime choices

## Demonstration Endpoints

Create endpoints that prove your implementation works:

1. **POST /tasks** - Creates a task and triggers notifications, validation, caching, and audit logging
2. **GET /tasks/{id}** - Retrieves a task (should use cache if available)
3. **GET /diagnostics** - Returns diagnostic information showing:
   - Current tenant ID
   - Total audit log count (from singleton)
   - Number of times various services have been instantiated
   - Cache statistics

## Bonus Challenges (+10 points each, max +20)

- Implement proper disposal patterns (IDisposable/IAsyncDisposable) for at least one service and demonstrate it works correctly
- Create a middleware that sets tenant context and show how scoped services use it consistently
- Implement service health checks that verify your DI configuration is valid

## Submission

Submit your complete project as files. I'll test:
- Whether services have correct lifetimes (singleton instances are reused, scoped instances are per-request, etc.)
- Whether tenant isolation works properly
- Whether advanced features (keyed services, factories, etc.) are implemented correctly
- Code organization and appropriate use of interfaces

## Grading Criteria

- **Correctness**: Services work as specified (40%)
- **Proper DI Usage**: Correct lifetimes, no service locator anti-patterns (30%)
- **Advanced Features**: Keyed services, factories, decoration (20%)
- **Code Quality**: Clean code, proper separation of concerns, comments (10%)

---

Good luck! This challenge will test your understanding of service lifetimes, dependency resolution, and real-world DI patterns. Feel free to research ASP.NET Core DI documentation—that's part of the learning process.