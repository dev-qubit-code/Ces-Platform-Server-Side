using Microsoft.EntityFrameworkCore;
using StudentsAssociation.Models;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions):base(dbContextOptions){}

}