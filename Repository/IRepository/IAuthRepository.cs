namespace MinimalApi;

public interface IAuthRepository
{
  Task<bool> IsUniqueUser(string email);
  Task RegisterUser(RegisterUserRequest request);
  Task<LoginUserResponse> LoginUser(LoginUserRequest request);

  Task<ICollection<ApplicationUser>> getUsers();
}
