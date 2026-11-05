using System.Security.Claims;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

public class ActivationCheckMiddleware(RequestDelegate next, IMemoryCache cache)
{
    private readonly RequestDelegate _next = next;
    public async Task InvokeAsync(HttpContext httpContext)
    {
         
        if(httpContext.User.Identity?.IsAuthenticated == true)
        {
            var userTokenActivation = bool.Parse(httpContext.User.FindFirstValue("Activation")!);

            if(userTokenActivation == true)
            {
                var userId = Guid.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                // check if the user in the cache get it
                var users = cache.Get<Dictionary<Guid, bool>>($"users-activations");
                
                // check user is deactivated in the cache
                if(users?[userId] != null && !users[userId])
                    return;
                
                await _next(httpContext);
            }  

            
        }
        else
          await _next(httpContext);
    }
}