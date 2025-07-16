namespace MinimalApi;

public class UpdateCouponRequest
{
  public string Name { get; set; }
  public bool IsActive { get; set; }
  public decimal Percent { get; set; }
}
