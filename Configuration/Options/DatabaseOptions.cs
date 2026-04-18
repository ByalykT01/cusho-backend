using System.ComponentModel.DataAnnotations;

namespace cusho.Configuration.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";
    [Required] public string Host { get; init; }
    [Required] public int Port { get; init; }
    [Required] public string Name { get; init; }
    [Required] public string User { get; init; }
    [Required] public string Password { get; init; }

    public string ToConnectionString =>
        $"Host={Host};Port={Port};Database={Name};Username={User};Password={Password};";
}