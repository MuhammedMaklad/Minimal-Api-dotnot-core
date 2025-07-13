namespace MinimalApi;

public class Coupon
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public decimal Percent { get; set; }
  public bool IsActive { get; set; }
  public DateTime? Created { get; set; }
  public DateTime? LastUpdated { get; set; }

  // Navigation Properties
  public string? ApplicationUserId { get; set; }
  public ApplicationUser? ApplicationUser { get; set; }

  public int? LocalUserId { get; set; }
  public LocalUser? LocalUser { get; set; }
}
