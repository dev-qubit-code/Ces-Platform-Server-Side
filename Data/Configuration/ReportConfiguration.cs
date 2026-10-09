using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Data.Configuration;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("Reports");

        builder.Property(r => r.TItle).HasMaxLength(50).IsRequired();

        builder.Property(r => r.Description).HasMaxLength(255).IsRequired();

        builder.Property(r => r.Priority).HasConversion<string>().IsRequired();
    }
}
