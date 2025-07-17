namespace MinimalApi;

public interface IAuthRepository
{
  Task<bool> IsUniqueUser(string email);
  Task<bool> RegisterUser(RegisterUserRequest request);
  Task <LoginUserResponse> LoginUser(LoginUserRequest request);
}
