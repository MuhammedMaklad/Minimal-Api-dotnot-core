using Microsoft.AspNetCore.Mvc;

namespace MinimalApi;

public static class AuthEndpoint
{
  public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/v1/auth")
    .WithTags("Auth");

    group.MapPost("/register", Register)
    .AddEndpointFilter<BasicValidator<RegisterUserRequest>>();
  }

  private static async Task<IResult> Register([FromBody] RegisterUserRequest request, IAuthRepository authRepository, ILogger<Program> logger)
  {
    if (!await authRepository.IsUniqueUser(request.Email))
      return TypedResults.BadRequest(BaseResponse<UserDto>.Failure("Error", "User Already Use"));
      
    if (await authRepository.RegisterUser(request))
      return TypedResults.Created("User Register Successfully, Please Login in", BaseResponse<UserDto>.OK(message: "Complete"));

    return TypedResults.Problem(
      detail: "Error While Register Process, Please Try Again Letter.",
      statusCode: StatusCodes.Status500InternalServerError
    );
  }
}
