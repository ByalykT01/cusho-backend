namespace cusho.Configuration;

public class JwtSettings
{
    public required string Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
}
