using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace cusho.Configuration.Extensions;

public static class ObservabilityExtension
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddObservability()
        {
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter())
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddOtlpExporter());

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
                logging.AddOtlpExporter();
            });

            return builder;
        }
    }
}