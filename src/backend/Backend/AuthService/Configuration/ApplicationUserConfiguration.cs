using AuthService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(u => u.UserId);

        builder.HasIndex(u => u.Login)
            .IsUnique();

        builder.Property(p => p.Login)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired();
        builder.Property(p => p.Surname)
            .IsRequired();
    }
}
