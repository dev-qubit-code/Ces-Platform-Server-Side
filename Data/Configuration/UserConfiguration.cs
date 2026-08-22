using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Data.Configuration;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasIndex(u => u.Email);

        builder.Property(i => i.Email).HasMaxLength(50).IsRequired();

        builder.Property(u => u.Name).HasMaxLength(50).IsRequired();

        builder.Property(u => u.Password).HasMaxLength(128).IsRequired();
        
        builder.Property(u => u.Role).HasConversion<string>().IsRequired();
    }
}
