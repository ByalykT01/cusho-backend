namespace cusho.Configuration.Extensions;

public static class DocumentationExtensions
{


    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddDocumentation()
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            return builder;
        }
    }

    extension(WebApplication app)
    {
        public IApplicationBuilder UseSwaggerWithDefaults()
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
    
            return app;
        }
    }
}