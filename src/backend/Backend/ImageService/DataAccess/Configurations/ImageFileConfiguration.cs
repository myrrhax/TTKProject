using ImageService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageService.DataAccess.Configurations;

public class ImageFileConfiguration : IEntityTypeConfiguration<ImageFile>
{
    public void Configure(EntityTypeBuilder<ImageFile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FilePath).IsRequired().HasMaxLength(255);
    }
}
