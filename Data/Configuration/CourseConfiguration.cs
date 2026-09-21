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
        
        builder.Property(i => i.Name).HasMaxLength(50).IsRequired();



        //  builder.Property(u => u.Tests).IsRequired();

        //  builder.Property(u => u.Notes).IsRequired();
    }
}
