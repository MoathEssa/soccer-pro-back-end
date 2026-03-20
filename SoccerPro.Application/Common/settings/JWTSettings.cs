public class JwtSettings
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpirationMinutes { get; set; }
    public string SecretKey { get; set; } = default!;
    public int RefreshTokenExpirationInDays { get; set; } = default!;
}