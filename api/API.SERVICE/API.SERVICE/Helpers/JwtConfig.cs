namespace API.SERVICE.Helpers;

public class JwtConfig
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public double DurationInMinutes { get; set; }
}