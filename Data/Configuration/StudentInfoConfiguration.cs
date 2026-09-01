using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Data.Configuration;

public class StudentInfoConfiguration : IEntityTypeConfiguration<StudentInfo>
{
    public void Configure(EntityTypeBuilder<StudentInfo> builder)
    {
        builder.ToTable("StudentsInfos");

        builder.Property(si => si.Name).HasMaxLength(50).IsRequired();

        builder.Property(si => si.About).HasMaxLength(255).IsRequired();

        builder.Property(si => si.Major).HasMaxLength(50).IsRequired();

        
    }
}
