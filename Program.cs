
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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

// * Register Auth Service
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
//* Register AutoMapper Service
builder.Services.AddAutoMapper(typeof(MappingProfiles));

// * Register Validators
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddScoped(typeof(BasicValidator<>));

// * Add Authentication Service
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Sets JWT as the default scheme for authentication
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Sets JWT as the default scheme for challenges 
}).AddJwtBearer(options =>
{
  options.RequireHttpsMetadata = false; // Disabled for development (set to true in production)
  options.SaveToken = true; //  Saves the token in the AuthenticationProperties
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
      builder.Configuration.GetValue<string>("ApiSettings:Secret")!)),
    ValidateIssuer = false,
    ValidateAudience = false
  };
});

// * Add Authorization Service
builder.Services.AddAuthorization(
  options =>
  {
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
  }
);

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
// ! AA Middleware 
app.UseAuthentication();
app.UseAuthorization();

// ! Custom Middleware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.MapGet("/", () => "Hello World from Coupon API!, Muhammed on da code ");


//! Map Endpoints
app.MapCouponEndpoints();
app.MapAuthEndpoints();

app.Run();


/*
  TODO:
  - run dotnet watch run [done]
  - test the API with Swagger UI [done]
  TODO:
  - Enable Logger [done]
  - Add Configuration [done]
  TODO: 
  - Add Logging Middleware [done]
  - Add Exception Handler Middleware [done]
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
  - Add Coupon endpoints [done]
  - Add Coupon repository [done]
  - Add Coupon Filter [done]
  - Add Coupon Validation [done] 
  TODO:
  - Add Auth model [done]
  - Add Auth repository [done]
  - Add Auth Endpoint [done]
  - Add Auth Filter [done]
  - Add Auth Validation [done]
  TODO:
  - Add Authentication
*/