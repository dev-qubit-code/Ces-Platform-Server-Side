using System.Text.Json.Serialization;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using SPMS_PROJECT.Exceptions;
using Microsoft.EntityFrameworkCore;
using SPMS_PROJECT.OpenApi.Transformers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ces_Platform_Server_Side.Validators;
using Ces_Platform_Server_Side.Interfaces;

namespace SPMS_PROJECT;


public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
                .AddCustomProblemDetails()
                .AddCustomApiVersioning()
                .AddApiDocumentation()
                .AddExceptionHandling()
                .AddController()
                .AddValidation()
                .AddDatabase(configuration)
                .AddCorsFunc()
                // .AddJwtAuthentication(configuration)
                // .AddAuthorizationPolicies()
                .AddBusinessServices();

        return services;
    }

    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = (context) =>
            {
                // add key value pair to the problem details
                //context.ProblemDetails.Extensions  
            };
        });

        return services;
    }


    public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        string[] versions = ["v1"];

        foreach (var version in versions)
        {
            services.AddOpenApi(version, options =>
            {
                // Versioning config
                options.AddDocumentTransformer<VersionInfoTransformer>();

                // Security Scheme config
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
            });
        }
        return services;
    }

    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }

    public static IServiceCollection AddController(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(op => 
        {
            op.UseSqlServer(configuration.GetConnectionString("Default"));
        }); 
        return services;
    }

    public static IServiceCollection AddCorsFunc(this IServiceCollection services)
    {
        services.AddCors(op => op.AddDefaultPolicy(bu => bu.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

        return services;
    }

    // public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    // {

    //     services.AddAuthentication(options =>
    //     {
    //         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //     }).AddJwtBearer(options =>
    //     {
    //         options.TokenValidationParameters = new TokenValidationParameters
    //         {
    //             ValidateIssuer = true,
    //             ValidateAudience = true,
    //             ValidateLifetime = true,
    //             ClockSkew = TimeSpan.Zero,
    //             ValidateIssuerSigningKey = true,
    //             ValidIssuer = "YourIssuer",
    //             ValidAudience = "YourAudiance",
    //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourIssuerSigningKey"))
    //         };
    //     });

    //     return services;
    // }

    // public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    // {
    //     services.AddAuthorization(options => {});
    //     return services;
    // }
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {        
        
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IUserService,UserService>();

        // services.AddScoped<IdentityService>();
        return services;
    }
}