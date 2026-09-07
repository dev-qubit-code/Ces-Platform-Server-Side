using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Data.Configuration;

public class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.ToTable("Sources");

        builder.Property(si => si.Name).HasMaxLength(50).IsRequired();

        builder.Property(si => si.Url).HasMaxLength(50).IsRequired();
    }
}
