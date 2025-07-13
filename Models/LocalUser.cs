namespace MinimalApi;

public class LocalUser
{
  public int Id { get; set; }
  public string UserName { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string Password { get; set; }   = string.Empty;
  public UserType UserType { get; set; }
}
