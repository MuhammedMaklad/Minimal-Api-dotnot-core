using Microsoft.AspNetCore.Identity;

namespace MinimalApi;

public class ApplicationUser : IdentityUser
{
  public string Name { get; set; } = string.Empty;
}
