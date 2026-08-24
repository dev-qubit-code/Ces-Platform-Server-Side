using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Interfaces;

public interface IUserRepository
{
    public Task<(int,List<User>)> GetUsersPageAsync(UserFilter? filter, CancellationToken ct = default);
    public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken ct = default);
    public  Task<bool> AddUserAsync(User user, CancellationToken ct = default);
    public Task<bool> UpdateUserAsync(CancellationToken ct = default);
    public Task<bool> DeleteUserAsync(Guid userId, CancellationToken ct = default);
    public Task<int> GetUsersCountAsync(CancellationToken ct = default);
}