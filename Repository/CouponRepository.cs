
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MinimalApi;

public class CouponRepository : ICouponRepository
{
  private readonly ApplicationDbContext _context;
  public CouponRepository(ApplicationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }
  public async Task CreateAsync(Coupon coupon)
  {
    await _context.Coupons.AddAsync(coupon);
  }

  public Task DeleteAsync(Coupon coupon)
  {
    _context.Coupons.Remove(coupon);
    return Task.CompletedTask;
  }

  public async Task<ICollection<Coupon>> GetAllCouponsAsync()
  {
    return await _context.Coupons.ToListAsync();
  }

  public async Task<Coupon?> GetAsync(int id)
  {
    return await _context.Coupons.FindAsync(id);
  }

  public  async Task<Coupon?> GetByCodeAsync(string codeName)
  {
    return await _context.Coupons.FirstOrDefaultAsync(c => c.Name.ToLower() == codeName.ToLower());
  }

  public async Task SaveAsync()
  {
    await _context.SaveChangesAsync();
  }

  public Task UpdateAsync(Coupon coupon)
  {
    _context.Coupons.Update(coupon);
    return Task.CompletedTask;
  }
}
