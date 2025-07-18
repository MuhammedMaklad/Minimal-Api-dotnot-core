namespace MinimalApi;

public class UserAlreadyExistsException : AuthException
{
  public UserAlreadyExistsException() : base("User Already Exists", StatusCodes.Status400BadRequest) { }
}
