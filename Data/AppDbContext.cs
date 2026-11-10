using Ces_Platform_Server_Side.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using System.Security.Policy;

public class AppDbContext : DbContext, IDataProtectionKeyContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<StudentInfo> StudentInfos { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses{ get; set; }
    public DbSet<Report> Reports{ get; set; }
    public DbSet<Note> Notes{ get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Log> Logs { get; set; }

    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions):base(dbContextOptions){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
 
    }

}