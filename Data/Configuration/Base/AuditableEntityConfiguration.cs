using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Data.Configuration;
public class AuditableEntityConfiguration : IEntityTypeConfiguration<AuditableEntity>
{
    public void Configure(EntityTypeBuilder<AuditableEntity> builder)
    {
        builder.Property(ae => ae.CreatedAtUtc).IsRequired();

        builder.Property(ae => ae.LastModifiedAtUtc).IsRequired();
        // string is NVARCHAR(MAX) by default 
        builder.Property(ae => ae.CreatedBy).HasMaxLength(50).IsRequired();

        builder.Property(ae => ae.LastModifiedBy).HasMaxLength(50).IsRequired();

    }
}
