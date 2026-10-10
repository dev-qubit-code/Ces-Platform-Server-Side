using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using System.Security.Claims;
using Ces_Platform_Server_Side.Enums;

public class UserService(IUserRepository repository, ILoggerWrapper<User> logger, IHttpContextAccessor accessor) : IUserService 
{
    public async Task<UserResponse> CreateUser(CreateUserRequest request, CancellationToken ct = default)
    {
        var principal = accessor.HttpContext!.User;

        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);        

        var newUser =  User.Create(request,username!);

            if(!await repository.AddUserAsync(newUser, ct))
            {
                logger.LogError($"Error occured while adding a new user {newUser.Id} by {username} at {DateTime.Now}", userRole);

                throw new InvalidOperationException("Error occured while adding the new user");
            }        
            
        logger.LogInformation($"Create new user {newUser.Id} by {username} at {newUser.CreatedAtUtc}",userRole);

        return UserResponse.FromModel(newUser);
    } 
    
    public async Task UpdateUser(Guid userId,UpdateUserRequest request, CancellationToken ct = default)
    {   
        var principal = accessor.HttpContext!.User;
        
        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);    

        var user = await repository.GetUserByIdAsync(userId);

        if(user is null)
        {
            logger.LogWarning($"User {userId} not found at {DateTime.Now}",userRole);

            throw new BusinessRuleException("User not found",StatusCodes.Status404NotFound);
        }

        if(user.IsEqual(request))
            return;

        user.Assign(request,username!);
 
        if(!await repository.UpdateUserAsync(ct))
        {
            logger.LogError($"Error occured while updating the user {user.Id} at {DateTime.Now} by {username}",userRole);

            throw new InvalidOperationException("Error occured while updating the user");
        }
        
        logger.LogInformation($"updated user {user.Id} by {username} at {user.LastModifiedAtUtc}",userRole);
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
        var user = await repository.GetUserByIdAsync(userId,ct);

        var principal = accessor.HttpContext!.User;
        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);
        
        if(user is null)
        {
            logger.LogWarning($"User {userId} not found at {DateTime.Now} requested by {username}",userRole);

            throw new BusinessRuleException("User not found",StatusCodes.Status404NotFound);
        }

        return UserResponse.FromModel(user);
    } 

    public async Task DeleteUser(Guid userId, CancellationToken ct = default)
    {
        var principal = accessor.HttpContext!.User;
        
        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);    

        var user = await repository.GetUserByIdAsync(userId);

        if(user is null)
        {
            logger.LogWarning($"User {userId} not found at {DateTime.Now} requested by {username}",userRole);

            throw new BusinessRuleException("User not found",StatusCodes.Status404NotFound);
        }

        if(!await repository.DeleteUserAsync(userId, ct))
        {
            logger.LogError($"Error occured while deleting the user {user.Id} at {DateTime.Now} by {username}",userRole);  

            throw new InvalidOperationException("Error occurd while deleting the user");
        }

        logger.LogInformation($"deleted user {user.Id} by {username} at {user.CreatedAtUtc}",userRole);
    }

    public async Task UpdateUserActivation(Guid userId, UpdateUserActivationRequest request, CancellationToken ct = default)
    {
        var principal = accessor.HttpContext!.User;
        
        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);    

        var user = await repository.GetUserByIdAsync(userId);

        if(user is null)
        {
            logger.LogWarning($"User {userId} not found at {DateTime.Now} requested by {username}",userRole);

            throw new BusinessRuleException("User not found",StatusCodes.Status404NotFound);
        }

        if(user.IsActive == request.IsActive)
            return;

        user.IsActive = request.IsActive;
        user.LastModifiedBy = username!;
        user.LastModifiedAtUtc = DateTimeOffset.UtcNow;
        
        if(!await repository.UpdateUserAsync(ct))
        {
            logger.LogError($"Error occured while updating the user activation {user.Id} at {DateTime.Now} by {username}",userRole);
            
            throw new InvalidOperationException("Error occured while updating the user activation");
        }

        logger.LogInformation($"updated user {user.Id} activation by {username} at {user.LastModifiedAtUtc}",userRole);
    }
}
