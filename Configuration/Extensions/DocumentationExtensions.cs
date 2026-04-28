using Microsoft.OpenApi;

namespace cusho.Configuration.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class DocumentationExtensions
{
    public static IHostApplicationBuilder AddDocumentation(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            const string securitySchemeName = "Bearer";

            options.AddSecurityDefinition(securitySchemeName, new OpenApiSecurityScheme
            {
                Description = "Enter JWT token in the format: Bearer {your token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", 
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(securitySchemeName, document)] = new List<string>(Array.Empty<string>())
            });
        });

        return builder;
    }

    public static IApplicationBuilder UseSwaggerWithDefaults(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
                options.RoutePrefix = "swagger";
                
                options.DisplayRequestDuration();
                options.EnableTryItOutByDefault();
            });
        }

        return app;
    }
}