using cusho.Configuration.Options;

namespace cusho.Configuration.Extensions;

public static class CorsExtensions
{
    public const string CorsPolicy = "Default";

    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddCorsPolicy()
        {
            var corsOptions = builder.Configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>();

            builder.Services.AddCors(options => options.AddPolicy(CorsPolicy, policy =>
            {
                if (corsOptions == null || corsOptions.AllowedOrigins.Equals("*"))
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                else
                    policy.WithOrigins(corsOptions.AllowedOrigins).AllowAnyMethod().AllowAnyHeader();
            }));
            return builder;
        }
    }
}