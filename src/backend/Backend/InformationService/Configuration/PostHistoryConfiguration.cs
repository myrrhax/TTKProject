using InformationService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InformationService.Configuration;

public class PostHistoryConfiguration : IEntityTypeConfiguration<PostHistory>
{
    public void Configure(EntityTypeBuilder<PostHistory> builder)
    {
        builder.HasKey(ph => ph.UpdateId);

        builder.HasOne(ph => ph.Post)
            .WithMany(p => p.History)
            .HasForeignKey(ph => ph.PostId);
    }
}
