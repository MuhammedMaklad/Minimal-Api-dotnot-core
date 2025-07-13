using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace MinimalApi;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Coupon> Coupons { get; set; } 
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<LocalUser> LocalUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Coupon>().ToTable("Coupons");
  }
}

