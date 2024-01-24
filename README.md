
# Global Error Handling and CRUD Operations for Drivers Table

## Overview

This repository implements a global error handling mechanism and simple CRUD functionality for a "drivers" table using Microsoft Entity Framework and SQL Server. The primary goal is to provide a robust middleware for handling exceptions, starting with custom exceptions such as:

- `NotFoundException`
- `KeyNotFoundException`
- `UnAuthorizeException`
- `NotImplementedException`
- `BadRequestException`

## Features

1. **Global Error Handling Middleware:**
   - The repository includes a full-featured middleware for handling exceptions globally in the system. It covers various custom exceptions, ensuring a comprehensive approach to error management.

2. **CRUD Operations for Drivers Table:**
   - Simple CRUD operations for the "drivers" table are implemented using Microsoft Entity Framework.
   - Each operation (Create, Read, Update, Delete) includes custom exception handling for better validation and error reporting.
## Middleware Implementation
### GlobalExceptionHandlingMiddleware Explanation

To implement the global error handling middleware, follow these steps:
1. Create a new class named `GlobalExceptionHandlingMiddleware`
2. Add the following code:

```csharp
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        var stackTrace = string.Empty;
        var msg = string.Empty;
        var result = new ApiErrorResponse();

        var exceptionType = exception.GetType();

        switch (exception)
        {
            //TODO: List all exceptions caused by the user
            case NotFoundException _:
            case KeyNotFoundException _:
                statusCode = HttpStatusCode.NotFound;
                result = new ApiErrorResponse(exception.Message);
                break;

            case BadRequestException _:
                statusCode = HttpStatusCode.BadRequest;
                result = new ApiErrorResponse(exception.Message);
                break;

            case UnAuthorizedAccessException _:
                statusCode = HttpStatusCode.Unauthorized;
                result = new ApiErrorResponse(exception.Message);
                break;

            case NotImplementedException _:
                statusCode = HttpStatusCode.NotImplemented;
                result = new ApiErrorResponse(exception.Message);
                break;

            case Exception ex:
                logger.LogError(exception, "SERVER ERROR");
                statusCode = HttpStatusCode.InternalServerError;
                result = string.IsNullOrWhiteSpace(ex.Message) ? new ApiErrorResponse("Error") : new ApiErrorResponse(ex.Message);
                break;
        }

        string exceptionResult = JsonSerializer.Serialize(result);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(exceptionResult);
    }
}
```
The GlobalExceptionHandlingMiddleware is a critical component for handling exceptions across the entire application. Let's break down its key components:
- The middleware is defined with the `RequestDelegate` and `ILogger<GlobalExceptionHandlingMiddleware>` dependencies, allowing it to process HTTP requests and log exceptions.
- The `Invoke` method is called for each HTTP request, and it wraps the request processing in a `try-catch` block. If an exception occurs during request processing, it invokes the `HandleExceptionAsync` method.
- The `HandleExceptionAsync` method determines the appropriate HTTP status code, generates a response based on the exception type, and logs server errors. It uses a switch statement to categorize different exceptions and sets the corresponding status code and response.

### Middleware Registration in Program.cs
To register the middleware in the Program.cs file using the provided extension method, follow these steps:
1. Add Extension Method Class:
```csharp
public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder AddGlobalErrorHandling(this IApplicationBuilder appBuilder)
    {
        return appBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
```
2. Call the extension method inside the program.cs :
```csharp
 app.AddGlobalErrorHandling();
```


## Code Examples

**Delete Driver Operation:**
- The `DeleteDriver` method checks for the existence of a driver in the database before deletion, throwing a `NotFoundException` if the driver is not found.

```csharp
public async Task<bool> DeleteDriver(int id)
{
    var driver = await _db.Drivers.FindAsync(id);
    
    if (driver is null)
        throw new NotFoundException($"Driver with id {id} not found!");

    _db.Drivers.Remove(driver);
    await _db.SaveChangesAsync();
    
    return true;
}
```

## API Endpoints

- **GetDriver:** Retrieve details of a specific driver.
- **GetDrivers:** Retrieve a list of all drivers.
- **CreateDriver:** Create a new driver entry.
- **UpdateDriver:** Update details of an existing driver.
- **DeleteDriver:** Delete a driver based on its ID.

## Response Examples

**Error ApiResponse Example:**
- The response includes details about the error, the type of exception, and a flag indicating failure.
```json
{
  "Message": "Driver with id 42 not found!",
  "Errors": "",
  "IsSuccess": false
}
```

**Successful ApiResponse Example:**
- The response includes a success message, a flag indicating success, and the relevant data if applicable.
```json
{
  "Message": "Driver details retrieved successfully.",
  "IsSuccess": true,
  "Value": {
    "Id": 1,
    "Name": "John Doe",
    "DriverNumber": "ABC123"
  }
}
```


## Contribution

Feel free to contribute by opening issues or submitting pull requests. Please ensure that any changes align with the project's goals and coding standards.

## License

This project is licensed under the [MIT License](LICENSE.md).

---

