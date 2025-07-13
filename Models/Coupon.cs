namespace MinimalApi;

public class Coupon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Percent { get; set; }
    public bool IsActive { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? LastUpdated { get; set; }

  public Coupon(string name, decimal percent, bool isActive)
  {
    Name = name;
    Percent = percent;
    IsActive = isActive;
  }
}
