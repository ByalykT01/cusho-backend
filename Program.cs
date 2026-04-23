using cusho.Configuration.Extensions;
using cusho.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();
builder.AddAppOptions();
builder.AddAppAuth();
builder.AddPersistence();
builder.AddCorsPolicy();
builder.AddObservability();
builder.AddAuthPolicies();

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddUserSecrets<Program>(optional: true);

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

var app = builder.Build();

await app.MigrateDatabaseAsync();

app.UseSwaggerWithDefaults();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHsts();
app.UseHttpsRedirection();
app.UseCors(CorsExtensions.CorsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();