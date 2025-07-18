namespace MinimalApi;

public class AuthException : Exception
{
  public int StatusCode { get; set; }

  public AuthException(string message, int statusCode = 400) : base(message) => StatusCode = statusCode;
}
