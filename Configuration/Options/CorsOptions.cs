namespace cusho.Configuration.Options;

public class CorsOptions
{
    public const string SectionName = "Cors";
    public string[] AllowedOrigins { get; init; } = [];
}