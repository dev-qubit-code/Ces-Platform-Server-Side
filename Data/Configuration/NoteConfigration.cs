using Ces_Platform_Server_Side.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ces_Platform_Server_Side.Data.Configuration
{
    public class NoteConfigration : IEntityTypeConfiguration<Note>

    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Notes");
            builder.HasIndex(n => n.Name).IsUnique();
            builder.Property(n => n.Date).HasColumnType("date");
            builder.Property(n => n.TeacherId).HasColumnType("UNIQUEIDENTIFIER");
            builder.Property(n => n.CourseId).HasColumnType("UNIQUEIDENTIFIER");

            builder.HasOne(n => n.Teacher)
                .WithMany(d => d.Notes);
            
        }
    }
}
