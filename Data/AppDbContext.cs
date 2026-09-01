using Ces_Platform_Server_Side.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<StudentInfo> StudentInfos { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions):base(dbContextOptions){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
 
    }

}