using InformationService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InformationService.Configuration;

public class PostsConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.PostId);

        builder.Property(p => p.Title)
            .IsRequired();
        builder.HasIndex(p => p.Title)
            .IsUnique();

        builder.Property(p => p.Content)
            .IsRequired();

        builder.HasMany(p => p.History)
            .WithOne(h => h.Post)
            .HasForeignKey(h => h.PostId);
    }
}
