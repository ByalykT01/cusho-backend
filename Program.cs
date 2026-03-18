using System.Net.Mime;
using System.Text;
using cusho.Data;
using cusho.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Load .env file in Development, use environment variables in Production
if (builder.Environment.IsDevelopment())
{
    Env.Load();
}

// Build configuration from environment variables
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") 
    ?? throw new InvalidOperationException("JWT_KEY environment variable is required");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "cusho";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "cusho";

var dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "cusho";
var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "";

var connectionString = string.IsNullOrEmpty(dbPassword)
    ? $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};"
    : $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};";

var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(',') 
    ?? ["http://localhost:5173"];

builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(connectionString));

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddProblemDetails();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = MediaTypeNames.Text.Plain;

            await context.Response.WriteAsync("An exception was thrown.");

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                await context.Response.WriteAsync(" The file was not found.");
            }

            if (exceptionHandlerPathFeature?.Path == "/")
            {
                await context.Response.WriteAsync(" Page: Home.");
            }
        });
    });

    app.UseHsts();
}

app.UseStatusCodePages();
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();