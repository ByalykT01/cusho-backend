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
    }
}