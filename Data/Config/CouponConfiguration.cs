namespace MinimalApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
  public void Configure(EntityTypeBuilder<Coupon> builder)
  {
    builder.ToTable("Coupons");

    builder.HasKey(c => c.Id);
    builder.Property(c => c.Id).ValueGeneratedOnAdd();

    builder.Property(c => c.Name)
      .IsRequired()
      .HasColumnType("nvarchar(100)")
      .HasMaxLength(100);

    builder.Property(c => c.Percent)
      .IsRequired()
      .HasColumnType("decimal(5,2)");

    builder.Property(c => c.IsActive)
      .HasColumnType("bit")
      .HasDefaultValue(true);

    builder.Property(c => c.Created)
    .HasColumnType("datetime")
    .HasDefaultValueSql("GETDATE()");

    builder.Property(c => c.LastUpdated)
      .HasColumnType("datetime")
      .HasDefaultValueSql("GETDATE()");


    // Configure the relationship
    builder.HasOne(c => c.ApplicationUser)
      .WithMany(u => u.Coupons)
      .HasForeignKey(c => c.ApplicationUserId)
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired(false); // Make it optional if needed


    // Seed initial data if needed
    builder.HasData(
      new Coupon
      {
        Id = 1,
        Name = "10% Off",
        Percent = 0.10m,
        IsActive = true,
        Created = DateTime.UtcNow,
        LastUpdated = DateTime.UtcNow,
      },
      new Coupon
      {
        Id = 2,
        Name = "20% Off",
        Percent = 0.20m,
        IsActive = true,
        Created = DateTime.UtcNow,
        LastUpdated = DateTime.UtcNow,
      }
    );
  }
}

