using Microsoft.AspNetCore.Diagnostics;

namespace cusho.Infrastructure;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetails) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex,
        CancellationToken ct)
    {
        logger.LogError(ex, "Unhandled exception for {Path}", ctx.Request.Path);
        ctx.Response.StatusCode = ex switch
        {
            FileNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return await problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = ctx,
            Exception = ex,
            ProblemDetails = { Title = "An error occured", Status = ctx.Response.StatusCode }
        });
    }
}