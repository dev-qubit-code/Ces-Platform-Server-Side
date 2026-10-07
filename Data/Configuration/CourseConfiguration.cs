using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Data.Configuration;


public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");

        builder.HasIndex(C => C.Name).IsUnique();
        
        builder.Property(c => c.Name).HasMaxLength(50).IsRequired();

        builder.HasMany(c => c.Notes).WithOne(c => c.Course).HasForeignKey(c => c.CourseId).IsRequired(false);

        builder.HasMany(c => c.Tests).WithOne(c => c.Course).HasForeignKey(c => c.CourseId).IsRequired(false);
    }
}
