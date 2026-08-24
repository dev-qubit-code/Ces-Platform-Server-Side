using Ces_Platform_Server_Side.Responses;

public interface IUserService
{
    public Task<UserResponse> CreateUser(CreateUserRequest request, CancellationToken ct = default);
    public Task UpdateUser(Guid userId,UpdateUserRequest request, CancellationToken ct = default);
    public Task<PagedResult<UserPageResponse>> GetPagedUsers(UserFilter? filter, CancellationToken ct = default);
    public Task<UserResponse> GetUserById(Guid userId,CancellationToken ct);
    public Task DeleteUser(Guid userId, CancellationToken ct = default);
}