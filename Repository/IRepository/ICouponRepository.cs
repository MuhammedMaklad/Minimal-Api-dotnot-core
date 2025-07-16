namespace MinimalApi;

public interface ICouponRepository
{
  Task<ICollection<Coupon>> GetAllCouponsAsync();
  Task<Coupon?> GetAsync(int id);
  Task<Coupon?> GetByCodeAsync(string codeName);
  Task CreateAsync(Coupon coupon);
  Task UpdateAsync(Coupon coupon);
  Task DeleteAsync(Coupon coupon);
  Task SaveAsync();
}
