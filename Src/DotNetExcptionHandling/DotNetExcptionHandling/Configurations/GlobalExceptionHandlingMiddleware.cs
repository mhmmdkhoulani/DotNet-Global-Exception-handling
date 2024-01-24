using DotNetExcptionHandling.Exceptions;
using DotNetExcptionHandling.Models;
using System.Net;
using System.Text.Json;
using KeyNotFoundException = DotNetExcptionHandling.Exceptions.KeyNotFoundException;
using NotImplementedException = DotNetExcptionHandling.Exceptions.NotImplementedException;

namespace DotNetExcptionHandling.Configurations
{
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
}
