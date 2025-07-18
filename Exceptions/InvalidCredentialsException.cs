namespace MinimalApi;

public class InvalidCredentialsException : AuthException
{
  public InvalidCredentialsException():base("InValid Credentials", StatusCodes.Status401Unauthorized)
  {
    
  }
}
