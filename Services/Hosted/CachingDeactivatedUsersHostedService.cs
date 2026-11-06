using Microsoft.Extensions.Caching.Memory;

namespace Ces_Platform_Server_Side.Services.Hosted;

public class CachingDeactivatedUsersHostedService(IServiceScopeFactory serviceScope, IMemoryCache cache) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using(var scope = serviceScope.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            var UsersActivations = context?.Users.Select(u => new { u.Id, u.IsActive}).ToDictionary(u => u.Id, u => u.IsActive)??
                throw new InvalidOperationException("Error occourd while caching the usersActivations");

            cache.Set("users-activations",UsersActivations);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}