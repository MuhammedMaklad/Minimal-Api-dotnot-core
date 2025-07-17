namespace MinimalApi;

public class RequestLoggingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<RequestLoggingMiddleware> _logger;
  public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var startTime = DateTime.UtcNow;

    await _next(context);
    var duration = DateTime.UtcNow - startTime;

    _logger.LogInformation($"Request to {context.Request.Path} took {duration.TotalMilliseconds}ms");
  }
}
