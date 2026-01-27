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

## Testing Guide

### Test Scenario 1: Service Lifetime Verification

**Purpose**: Verify that Singleton, Scoped, and Transient lifetimes work correctly.

**Test Steps**:

1. Start the application
2. Make **Request A** to `GET /diagnostics`
3. Make **Request B** to `GET /diagnostics`
4. Make **Request C** to `GET /diagnostics`

**Expected Results**:

- **Singleton (`IAuditLogger`)**:
    - Should show increasing operation count across all requests
    - Request A: `totalOperations: 1`
    - Request B: `totalOperations: 2`
    - Request C: `totalOperations: 3`
    - Same instance ID across all requests

- **Scoped (`ITenantProvider`, `ITaskRepository`)**:
    - Should have the same instance within a single request
    - Should have different instances across different requests
    - Each request should maintain consistent tenant ID throughout its lifecycle

- **Transient (`INotificationService`)**:
    - Should create new instance every time it's injected
    - Different notification IDs even within the same request if injected multiple times

### Test Scenario 2: Tenant Isolation

**Purpose**: Verify that different tenants see only their own data.

**Test Data**:

**Request 1** (Tenant A):

```http
POST /tasks
X-Tenant-Id: tenant-a
Content-Type: application/json

{
  "title": "Task A1",
  "description": "First task for Tenant A",
  "type": "urgent"
}
```

**Request 2** (Tenant A):

```http
POST /tasks
X-Tenant-Id: tenant-a
Content-Type: application/json

{
  "title": "Task A2",
  "description": "Second task for Tenant A",
  "type": "scheduled"
}
```

**Request 3** (Tenant B):

```http
POST /tasks
X-Tenant-Id: tenant-b
Content-Type: application/json

{
  "title": "Task B1",
  "description": "First task for Tenant B",
  "type": "recurring"
}
```

**Expected Results**:

- Tenant A should see 2 tasks (Task A1, Task A2)
- Tenant B should see 1 task (Task B1)
- Getting tasks for Tenant A should NOT return Task B1
- Getting tasks for Tenant B should NOT return Task A1 or Task A2

### Test Scenario 3: Caching Behavior

**Purpose**: Verify that the caching layer works and is tenant-aware.

**Test Steps**:

1. **Request 1**: `POST /tasks` with Tenant A - creates task with ID "task-1"
2. **Request 2**: `GET /tasks/task-1` with Tenant A (cache miss)
3. **Request 3**: `GET /tasks/task-1` with Tenant A (should hit cache)
4. **Request 4**: `GET /tasks/task-1` with Tenant B (should NOT return cached data from Tenant A)

**Expected Results**:

- Request 2: Cache miss, retrieves from repository, stores in cache
- Request 3: Cache hit, returns same data without hitting repository
- Request 4: Either returns 404 (task doesn't belong to Tenant B) or cache miss (separate cache per tenant)
- Diagnostics endpoint should show:
    - Cache hits: 1 (after Request 3)
    - Cache misses: 1 or 2 (depending on tenant isolation)

### Test Scenario 4: Keyed Services (Notification Strategies)

**Purpose**: Verify that different notification types are resolved correctly.

**Test Data**:

Configure in `appsettings.json`:

```json
{
    "TenantSettings": {
        "tenant-a": {
            "preferredNotification": "Email"
        },
        "tenant-b": {
            "preferredNotification": "Sms"
        },
        "tenant-c": {
            "preferredNotification": "Push"
        }
    }
}
```

**Test Steps**:

1. Create task for Tenant A
2. Create task for Tenant B
3. Create task for Tenant C

**Expected Results**:

- Tenant A task triggers EmailNotification (check logs/response)
- Tenant B task triggers SmsNotification (check logs/response)
- Tenant C task triggers PushNotification (check logs/response)
- Each notification should have different implementation-specific output

### Test Scenario 5: Factory Pattern (Task Processors)

**Purpose**: Verify that the factory creates different processors based on task type.

**Test Data**:

```json
// Urgent Task
{
  "title": "Critical Bug",
  "description": "Production is down",
  "type": "urgent"
}

// Scheduled Task
{
  "title": "Weekly Report",
  "description": "Generate weekly metrics",
  "type": "scheduled"
}

// Recurring Task
{
  "title": "Daily Backup",
  "description": "Backup database",
  "type": "recurring"
}
```

**Expected Results**:

- Urgent task: Processed by `UrgentTaskProcessor` (immediate handling, high priority)
- Scheduled task: Processed by `ScheduledTaskProcessor` (queued for future execution)
- Recurring task: Processed by `RecurringTaskProcessor` (sets up recurring schedule)
- Each processor should log/return different processing behavior

### Test Scenario 6: Validation

**Purpose**: Verify that task validation works correctly.

**Test Data**:

**Valid Task**:

```json
{
    "title": "Valid Task",
    "description": "This is a properly formatted task",
    "type": "urgent"
}
```

**Invalid Tasks**:

```json
// Missing title
{
  "description": "No title provided",
  "type": "urgent"
}

// Invalid type
{
  "title": "Bad Type",
  "description": "Using invalid task type",
  "type": "invalid-type"
}

// Empty description
{
  "title": "No Description",
  "description": "",
  "type": "scheduled"
}
```

**Expected Results**:

- Valid task: Returns 201 Created with task details
- Missing title: Returns 400 Bad Request with validation error
- Invalid type: Returns 400 Bad Request with validation error
- Empty description: Returns 400 Bad Request with validation error

### Test Scenario 7: Configuration-Based Behavior

**Purpose**: Verify that services change behavior based on configuration.

**Test Steps**:

1. Set `AuditLogLevel` to "Detailed" in appsettings.json
2. Create a task
3. Check diagnostics - should show detailed audit logs
4. Change `AuditLogLevel` to "Summary"
5. Restart application
6. Create another task
7. Check diagnostics - should show summary audit logs

**Expected Results**:

- Detailed mode: Logs include timestamp, tenant, operation, full details
- Summary mode: Logs include only operation count and basic info

### Test Scenario 8: Concurrent Requests (Scoped Verification)

**Purpose**: Verify that scoped services maintain isolation across concurrent requests.

**Test Steps**:

1. Send 5 concurrent requests to `GET /diagnostics` with different tenant IDs
2. Each request should complete independently

**Expected Results**:

- Each response shows its own tenant ID (no cross-contamination)
- Scoped services in Request A don't affect Request B
- Singleton audit logger correctly increments by 5 total

### Bonus Test Scenarios

**Test Scenario 9: Disposal Pattern (If Implemented)**

**Test Steps**:

1. Make a request that uses a disposable service
2. Request completes
3. Check logs/diagnostics for disposal confirmation

**Expected Results**:

- Service's Dispose/DisposeAsync method is called at end of scope
- Resources are properly cleaned up
- Logs show "Service [name] disposed" or similar

**Test Scenario 10: Middleware Tenant Context (If Implemented)**

**Test Steps**:

1. Send request WITHOUT X-Tenant-Id header
2. Send request WITH X-Tenant-Id header

**Expected Results**:

- Without header: Default tenant or 400 Bad Request
- With header: Tenant context is set and used throughout request pipeline

---

## Sample API Test Collection

You can use this cURL command set or import into Postman:

```bash
# Test 1: Create task for Tenant A
curl -X POST http://localhost:5000/tasks \
  -H "X-Tenant-Id: tenant-a" \
  -H "Content-Type: application/json" \
  -d '{"title":"Task A1","description":"First task","type":"urgent"}'

# Test 2: Get task for Tenant A
curl -X GET http://localhost:5000/tasks/1 \
  -H "X-Tenant-Id: tenant-a"

# Test 3: Try to get Tenant A's task as Tenant B (should fail)
curl -X GET http://localhost:5000/tasks/1 \
  -H "X-Tenant-Id: tenant-b"

# Test 4: Get diagnostics
curl -X GET http://localhost:5000/diagnostics \
  -H "X-Tenant-Id: tenant-a"

# Test 5: Test validation (should fail)
curl -X POST http://localhost:5000/tasks \
  -H "X-Tenant-Id: tenant-a" \
  -H "Content-Type: application/json" \
  -d '{"description":"Missing title","type":"urgent"}'
```

---

Good luck! This challenge will test your understanding of service lifetimes, dependency resolution, and real-world DI patterns. Feel free to research ASP.NET Core DI documentation—that's part of the learning process.
