using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cusho.Infrastructure;

public sealed class ProblemAuthorizationMiddlewareResultHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<ProblemAuthorizationMiddlewareResultHandler> logger)
    : IAuthorizationMiddlewareResultHandler
{
    private static readonly AuthorizationMiddlewareResultHandler DefaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Challenged)
        {
            logger.LogInformation(
                "Authentication challenge on {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await WriteProblemAsync(context, ProblemDescriptors.Unauthorized);
            return;
        }

        if (authorizeResult.Forbidden)
        {
            var userId = context.User.FindFirst("sub")?.Value ?? "anonymous";

            logger.LogWarning(
                "Authorization denied on {Method} {Path} for user {UserId}",
                context.Request.Method,
                context.Request.Path,
                userId);

            await WriteProblemAsync(context, ProblemDescriptors.Forbidden);
            return;
        }

        await DefaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }

    private async Task WriteProblemAsync(HttpContext context, ProblemDescriptor descriptor)
    {
        context.Response.StatusCode = descriptor.StatusCode;

        if (descriptor.StatusCode == StatusCodes.Status401Unauthorized)
        {
            context.Response.Headers.WWWAuthenticate = "Bearer";
        }

        var problemDetails = new ProblemDetails
        {
            Status = descriptor.StatusCode,
            Title = descriptor.Title,
            Detail = descriptor.Detail,
            Type = descriptor.Type,
            Instance = context.Request.Path,
        };

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        var written = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = problemDetails,
        });

        if (!written)
        {
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}