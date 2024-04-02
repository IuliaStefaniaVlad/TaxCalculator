using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ErrorHandlingSample;
/// <summary>
/// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0
/// https://www.c-sharpcorner.com/article/error-handling-in-net-core-web-api-with-custom-middleware/
/// </summary>
public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> logger;
    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        this.logger = logger;
    }
    public  ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;
        logger.LogError(
            "{Id}, Error Message: {exceptionMessage}, Time of occurrence {time}",
            Activity.Current?.Id, exceptionMessage, DateTime.UtcNow);

        var problemDetails = new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = (int)StatusCodes.Status500InternalServerError,
            Detail = $"Something went wrong :( Sorry for this, damn. Use this id to get help {Activity.Current?.Id}",
            
        };

         httpContext.Response.WriteAsJsonAsync(problemDetails);

        // Return false to continue with the default behavior
        // - or - return true to signal that this exception is handled

        return ValueTask.FromResult(false);
    }
}