using cusho.Services;
using cusho.Models;
using Microsoft.AspNetCore.Authorization;
using cusho.Infrastructure;

namespace cusho.Configuration.Extensions;

public static class ServicesExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddServices()
        {
            builder.Services.AddScoped<UsersService>();
            builder.Services.AddScoped<AuthService>();
            return builder;
        }

        public IHostApplicationBuilder AddAuthPolicies()
        {
            builder.Services
                .AddAuthorizationBuilder()
                .AddPolicy("IsAdmin", policy => policy.RequireRole(nameof(Role.Admin)));

            builder.Services.AddSingleton<
                IAuthorizationMiddlewareResultHandler,
                ProblemAuthorizationMiddlewareResultHandler>();

            return builder;
        }
    }
}