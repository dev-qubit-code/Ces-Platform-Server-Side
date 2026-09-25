using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Data.Configuration;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();

        builder.HasMany(t => t.Notes).WithOne(n => n.Teacher).HasForeignKey(n => n.TeacherId).IsRequired(false);

        builder.HasMany(t => t.Tests).WithOne(n => n.Teacher).HasForeignKey(n => n.TeacherId).IsRequired(false);


    }
}
