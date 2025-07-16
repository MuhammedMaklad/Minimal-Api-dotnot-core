namespace MinimalApi;

public class CreateCouponRequest
{
  public string Name { get; set; } = string.Empty;
  public decimal Percent { get; set; }
  public bool IsActive { get; set; }
}
