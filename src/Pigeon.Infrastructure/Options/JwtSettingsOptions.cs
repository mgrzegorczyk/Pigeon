namespace Pigeon.Infrastructure.Options;

public class JwtSettingsOptions()
{
    public string SecretKey { get; init; }
    public int ExpiryInMinutes { get; init; }
}