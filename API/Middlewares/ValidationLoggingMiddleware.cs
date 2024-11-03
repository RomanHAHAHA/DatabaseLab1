using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DatabaseLab1.API.Middlewares;

public class ValidationLoggingMiddleware(
RequestDelegate next,
ILogger<ValidationLoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ValidationLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        context.Request.Body.Position = 0;

        using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
        {
            var body = await reader.ReadToEndAsync();
            _logger.LogInformation($"Request Body:\n{body}");
            context.Request.Body.Position = 0;
        }

        await _next(context);

        if (context.Response.StatusCode == 400 &&
            context.Items["ModelState"] is ModelStateDictionary modelState)
        {
            foreach (var error in modelState)
            {
                _logger.LogError($"Validation error for field {error.Key}: " +
                    $"{string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }
        }
    }
}
