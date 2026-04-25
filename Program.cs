using System.Text.Json.Serialization;
using cusho.Configuration.Extensions;
using cusho.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddUserSecrets<Program>(optional: true);

builder.AddServices();
builder.AddAppOptions();
builder.AddAppAuth();
builder.AddPersistence();
builder.AddCorsPolicy();
builder.AddObservability();
builder.AddDocumentation();
builder.AddAuthPolicies();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

var app = builder.Build();

await app.MigrateDatabaseAsync();

app.UseExceptionHandler();
app.UseSwaggerWithDefaults();
app.UseHttpsRedirection();
app.UseHsts();
app.UseCors(CorsExtensions.CorsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();
