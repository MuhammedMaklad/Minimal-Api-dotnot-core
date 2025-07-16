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
    var response = new BaseResponse<CouponDto>()
    {
      Success = false,
    };

    var contextObj = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(T));

    if (contextObj is null)
      return Results.BadRequest(new
      {
        IsSuccess = false,
        StatusCode = HttpStatusCode.BadRequest
      });

    var result = await _validator.ValidateAsync((T)(contextObj));

    if (!result.IsValid)
    {
      response.Message = result.Errors?.FirstOrDefault()?.ToString() ?? "Validation Failed";
      return Results.BadRequest(response);
    }
    return await next(context);
  }
}
