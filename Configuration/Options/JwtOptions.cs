using System.ComponentModel.DataAnnotations;

namespace cusho.Configuration.Options;

public sealed class JwtOptions
{
    public const string SectionName = "JWT";
    [Required] public string Key { get; init; }
    [Required] public string Issuer { get; init; }
    [Required] public string Audience { get; init; }
}