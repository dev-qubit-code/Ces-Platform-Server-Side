using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Data.Configuration;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable("Logs");

        builder.Property(l => l.Message).IsRequired();
        
        builder.Property(l => l.Role).HasConversion<string>().IsRequired();

        builder.Property(l => l.CreatedAtUtc).IsRequired();
    }
}
