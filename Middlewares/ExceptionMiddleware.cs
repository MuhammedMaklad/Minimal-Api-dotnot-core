namespace MinimalApi;

public class ExceptionMiddleware
{
  private readonly RequestDelegate next;
  private readonly ILogger<ExceptionMiddleware> logger;

  public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
  {
    this.next = next;
    this.logger = logger;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await next(context);
    }
    catch (Exception ex)
    {
      logger.LogError(ex.Message);
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      await context.Response.WriteAsJsonAsync(new
      {
        Success = false,
        Error = "An error occurred while processing your request"
      });
    }
  }
}
