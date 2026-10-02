using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Data.Configuration;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");

        builder.Property(a => a.Title).HasMaxLength(50).IsRequired();

        builder.Property(a => a.Description).HasMaxLength(15000).IsRequired();

        builder.Property(a => a.Date).IsRequired();

        builder.Property(a => a.ThumbnailUrl).HasMaxLength(255).IsRequired(false);
    }
}
