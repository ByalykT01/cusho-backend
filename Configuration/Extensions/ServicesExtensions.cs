using cusho.Services;

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
            builder.Services.AddAuthorizationBuilder().AddPolicy("IsAdmin", policy => { policy.RequireRole("2"); });
            return builder;
        }
    }
}