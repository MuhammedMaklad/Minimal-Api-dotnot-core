using FluentValidation;
using System.Net;

namespace MinimalApi;

public class BasicValidator<T> : IEndpointFilter where T : class
{
  private readonly IValidator<T> _validator;
  private ILogger<BasicValidator<T>> _logger;
  public BasicValidator(IValidator<T> validator, ILogger<BasicValidator<T>> logger)
  {
    _validator = validator;
    _logger = logger;
  }
  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var requestObject = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(T));

        if (requestObject is null)
        {
            _logger.LogWarning("No object of type {Type} found in request context", typeof(T).Name);
            return Results.BadRequest(BaseResponse<T>.Failure("Invalid request format"));
        }

        var validationResult = await _validator.ValidateAsync((T)requestObject);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
            
            _logger.LogInformation("Validation failed for {Type}: {Errors}", 
                typeof(T).Name, 
                string.Join(", ", errorMessages));

            return Results.BadRequest(BaseResponse<T>.Failure(
                "Validation failed", 
                errorMessages
            ));
        }

        return await next(context);
    }
}
