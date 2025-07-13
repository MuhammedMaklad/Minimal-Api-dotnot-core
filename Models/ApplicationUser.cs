using Microsoft.AspNetCore.Identity;

namespace MinimalApi;

public class ApplicationUser : IdentityUser
{
  public string Name { get; set; } = string.Empty;

  public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
}
