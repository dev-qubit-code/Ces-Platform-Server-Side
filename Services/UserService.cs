using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using SPMS_PROJECT.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;

public class UserService(IUserRepository repository) : IUserService 
{
    public async Task<UserResponse> CreateUser(CreateUserRequest request, CancellationToken ct = default)
    {
        
        var user =  User.Create(request,"testName");

        if(!await repository.AddUserAsync(user,ct))
            throw new InvalidOperationException("Error occured while adding the user");

        return UserResponse.FromModel(user);
    } 
    
    public async Task UpdateUser(Guid userId,UpdateUserRequest request, CancellationToken ct = default)
    {   
        var user = await repository.GetUserByIdAsync(userId);

        if(user is null)
            throw new BusinessRuleException("User not found",StatusCodes.Status404NotFound);

        if(user.IsEqual(request))
            throw new BusinessRuleException("user already updated",StatusCodes.Status409Conflict);

        user.Assign(request,"testName");
 
        if(!await repository.UpdateUserAsync(ct))
            throw new InvalidOperationException("Error occured while updating the user");
    } 

    public async Task<PagedResult<UserPageResponse>> GetPagedUsers(UserFilter? filter, CancellationToken ct = default)
    {
         
        
        (int totalCount,var users) = await repository.GetUsersPageAsync(filter, ct);

        filter ??= new();

        if(users is null || !users.Any()) 
            return PagedResult<UserPageResponse>.Create(
            [],
            totalCount,
            filter.Page,
            filter.PageSize);

        var pagedResult = PagedResult<UserPageResponse>.Create(
            UserPageResponse.FromModels(users),
            totalCount,
            filter.Page,
            filter.PageSize);

        return pagedResult;
    }
    public async Task<UserResponse> GetUserById(Guid userId,CancellationToken ct)
    {
        var user = await repository.GetUserByIdAsync(userId,ct) ?? throw new BusinessRuleException("User not found",StatusCodes.Status404NotFound); 

        return UserResponse.FromModel(user);
    } 

    public async Task DeleteUser(Guid userId, CancellationToken ct = default)
    {
        var success = await repository.DeleteUserAsync(userId, ct);

        if(!success) 
            throw new InvalidOperationException("Error occurd while deleting the user");
    }

    public async Task UpdateUserActivation(Guid userId, UpdateUserActivationRequest request, CancellationToken ct = default)
    {
        var user = await repository.GetUserByIdAsync(userId,ct) ?? throw new BusinessRuleException("User not found",StatusCodes.Status404NotFound);

        if(user.IsActive == request.IsActive)
            throw new BusinessRuleException($"The user is already {(user.IsActive?"Activated":"Deactivated")}",StatusCodes.Status409Conflict);

        user.IsActive = request.IsActive;
        
        if(!await repository.UpdateUserAsync(ct))
            throw new InvalidOperationException("Error occured while updating the user activation");
    }
}