public class AppSettings
{
    public required EmailSettings EmailSettings { get; set; }
    public required JwtSettings JwtSettings { get; set; }
}
public class EmailSettings
{
    public required string SmtpServer { get; set; }
    public required int SmtpPort { get; set; }
    public required string SenderName { get; set; }
    public required string SenderEmail { get; set; }
    public required string SenderPassword { get; set; }
}

public class JwtSettings
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string SecretKey { get; set; }
    public required int MagicTokenExpirationInMinutes { get; set; }
    public required int AccessTokenExpirationInMinutes { get; set; }
}
