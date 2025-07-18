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
    group.MapPost("/login", Login)
    .AddEndpointFilter<BasicValidator<LoginUserRequest>>();

    group.MapGet("/fetchUser", async (IAuthRepository repo) =>
    {
      return TypedResults.Ok(new
      {
        users = await repo.getUsers()
      });
    }).RequireAuthorization("Admin");
  }

  private static async Task<IResult> Register([FromBody] RegisterUserRequest request, IAuthRepository authRepository, ILogger<Program> logger)
  {
    await authRepository.RegisterUser(request);

    return TypedResults.Created("User Register Successfully, Please Login in", BaseResponse<UserDto>.OK(message: "Complete"));
  }

  private static async Task<IResult> Login([FromBody] LoginUserRequest request, IAuthRepository authRepository, ILogger<Program> logger)
  {
    var response = await authRepository.LoginUser(request);
    return TypedResults.Ok(response);
  }
}
