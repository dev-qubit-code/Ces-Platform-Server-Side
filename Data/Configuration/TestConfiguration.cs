using Ces_Platform_Server_Side.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ces_Platform_Server_Side.Data.Configuration
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("Tests");
             
            builder.Property(n => n.Date).HasColumnType("date");
            builder.Property(n => n.TeacherId).HasColumnType("UNIQUEIDENTIFIER");
            builder.Property(n => n.CourseId).HasColumnType("UNIQUEIDENTIFIER");

            builder.HasOne(n => n.Teacher)
                .WithMany(d => d.Tests);

            builder.HasOne(n => n.Course)
                .WithMany(d => d.Tests);

        }
    }
}
