using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MinimalApi;

public class LocalUserConfiguration : IEntityTypeConfiguration<LocalUser>
{
  public void Configure(EntityTypeBuilder<LocalUser> builder)
  {
    builder.ToTable("LocalUsers");

    builder.HasKey(u => u.Id);
    builder.Property(u => u.Id).ValueGeneratedOnAdd();

    builder.Property(u => u.UserName)
      .IsRequired()
      .HasColumnType("nvarchar(50)")
      .HasMaxLength(50);

    builder.Property(u => u.Name)
      .IsRequired()
      .HasColumnType("nvarchar(100)")
      .HasMaxLength(100);

    builder.Property(u => u.Password)
      .IsRequired()
      .HasColumnType("nvarchar(255)")
      .HasMaxLength(255);

    builder.Property(u => u.UserType)
      .IsRequired();


    // Relationship configuration
    builder.HasMany(u => u.Coupons)
      .WithOne(c => c.LocalUser)
      .HasForeignKey(c => c.LocalUserId)
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired(false);
  }
}
