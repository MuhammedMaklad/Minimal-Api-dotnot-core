
namespace MinimalApi;

public class ParameterIdValidator : IEndpointFilter
{
  private ILogger<ParameterIdValidator> _logger;

  public ParameterIdValidator(ILogger<ParameterIdValidator> logger) => _logger = logger;
  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
  {
    _logger.LogInformation("inter Params filters");
    try
    {
      var id = context.GetArgument<int>(0) as int?; // Get raw string

      if (id is null ||id  <= 0)
      {
        _logger.LogWarning($"Invalid ID format: {id}");
        return Results.BadRequest(new
        {
          Error = "Invalid ID format",
          Message = "ID must be a positive integer",
          ReceivedValue = id
        });
      }

      // Replace the string argument with parsed int
      // context.Arguments[0] = parsedId;

      return await next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Unexpected validation error");
      return Results.Problem(
          statusCode: StatusCodes.Status500InternalServerError,
          title: "Validation error",
          detail: ex.Message);
    }
  }
}
