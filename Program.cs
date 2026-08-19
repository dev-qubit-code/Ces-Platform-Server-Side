using SPMS_PROJECT;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

if(!app.Environment.IsDevelopment())
    app.UseExceptionHandler();

// app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "CES-Platform Api");

        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.EnableFilter();
    });
}

app.UseStatusCodePages();

app.UseRouting();

// app.UseCors();

// app.UseAuthentication();

// app.UseAuthorization();

app.MapControllers();

app.Run();
