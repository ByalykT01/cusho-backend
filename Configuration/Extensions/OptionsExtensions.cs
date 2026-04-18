using cusho.Configuration.Options;

namespace cusho.Configuration.Extensions;

public static class OptionsExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddAppOptions()
        {
            builder.Services.AddOptions<CorsOptions>()
                .Bind(builder.Configuration.GetSection(CorsOptions.SectionName)).ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddOptions<DatabaseOptions>()
                .Bind(builder.Configuration.GetSection(DatabaseOptions.SectionName)).ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddOptions<JwtOptions>()
                .Bind(builder.Configuration.GetSection(JwtOptions.SectionName)).ValidateOnStart();

            return builder;
        }
    }
}