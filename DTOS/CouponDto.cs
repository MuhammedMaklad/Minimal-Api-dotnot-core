namespace MinimalApi;

public class CouponDto
{
  public string Name { get; set; } = string.Empty;
  public decimal Percent { get; set; }
  public bool IsActive { get; set; }
}
