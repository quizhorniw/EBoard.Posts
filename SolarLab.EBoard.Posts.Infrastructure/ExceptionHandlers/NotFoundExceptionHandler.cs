using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SolarLab.EBoard.Posts.Infrastructure.ExceptionHandlers;

public class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly Logger<NotFoundExceptionHandler> _logger;
    
    private readonly IEnumerable<Type> _notFoundExceptions =
    [
        typeof(KeyNotFoundException)
    ];
    
    public NotFoundExceptionHandler(Logger<NotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (!_notFoundExceptions.Contains(exception.GetType()))
        {
            return false;
        }

        _logger.LogError(exception, "Exception occured: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not found",
            Detail = exception.Message
        };
        
        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}