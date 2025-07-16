
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi;

var builder = WebApplication.CreateBuilder(args);

// *[1] Configuring Logging
builder.Logging.ClearProviders(); 
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Model", LogLevel.None);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Query", LogLevel.None);


// *[2] Configuring Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();


// *[3] Configuring Services
// * Registers Entity Framework Core database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// * Registers Identity services for user management
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// * Registers the Coupon repository
builder.Services.AddScoped<ICouponRepository, CouponRepository>();

//* Register AutoMapper Service
builder.Services.AddAutoMapper(typeof(MappingProfiles));

// * Register Validators
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddScoped(typeof(BasicValidator<>));


// * Service for Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
  config.DocumentName = "Coupon API";
  config.Title = "Coupon v1";
  config.Version = "v1";
});


var app = builder.Build();


// * Logging is now available
app.Logger.LogInformation("Application is starting up...");

if (app.Environment.IsDevelopment())
{
  app.UseOpenApi();
  app.UseSwaggerUi(config =>
  {
    config.DocumentTitle = "Coupon API";
    config.Path = "/swagger";
    config.DocumentPath = "/swagger/{documentName}/swagger.json";
    config.DocExpansion = "list";
  }
  );
}

app.MapGet("/", () => "Hello World from Coupon API!, Muhammed on da code ");
// Map the coupon endpoints
app.MapCouponEndpoints();
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";
        
        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        
        await context.Response.WriteAsJsonAsync(new 
        {
            Error = error?.Message ?? "Bad request",
            Details = error is BadHttpRequestException ? "Invalid parameter format" : null
        });
    });
});
app.Run();


/*
  TODO:
  - run dotnet watch run
  - test the API with Swagger UI
  TODO:
  - Enable Logger [done]
  - Add Configuration [done]
  TODO:
  - Create a database [done]
  - Add a database connection [done]
  - configure Entity Framework Core [done]
  - configure Identity [done]
  TODO:
  - Setup Global Error Handler [done]
  - Setup Request & Response [done]
  TODO:
  - Add Coupon model  [done]
  - Add Coupon endpoint [done]
  - Add Coupon repository [done]
  - Add Coupon Filter [done]
  - Add Coupon Validation [] 
  
*/