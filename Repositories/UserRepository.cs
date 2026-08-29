using Ces_Platform_Server_Side.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<(int,List<User>)> GetUsersPageAsync(UserFilter? filter, CancellationToken ct = default)
    {
        IQueryable<User> users = context.Users;

        List<User> page;
        int totalCount;

        if(filter is null)
        {
            page = await users.Take(10).ToListAsync(ct);

            totalCount = await users.CountAsync();
            return (totalCount,page);
        }

        filter.PageSize = Math.Max(1, filter.PageSize);
        filter.Page = Math.Clamp(filter.Page, 1, 100);

        if(!string.IsNullOrWhiteSpace(filter.Search))
            users = users.Where(u =>
                     u.Name.Contains(filter.Search) || 
                     u.Email.Contains(filter.Search) || 
                     u.CreatedAtUtc.ToString().Contains(filter.Search)  
                );

        totalCount = await users.CountAsync(ct);

        page = await users.Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .ToListAsync(ct);
        
        return (totalCount,page);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
    }

    public async Task<bool> AddUserAsync(User user, CancellationToken ct = default)
    {
        context.Users.Add(user);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateUserAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct) > 0;

    public async Task<bool> DeleteUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user == null)
            return false;

        context.Users.Remove(user);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<int> GetUsersCountAsync(CancellationToken ct = default) => await context.Users.CountAsync(ct);
}