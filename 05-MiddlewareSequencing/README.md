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

```
