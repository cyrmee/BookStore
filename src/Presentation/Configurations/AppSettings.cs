namespace Presentation.Configurations;
#pragma warning disable CS8618
public record AppSettings
{
    public string BaseUrl { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public Logging Logging { get; set; }
    public string AllowedHosts { get; set; }
    public JwtBearer JwtBearer { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}

public class Logging
{
    public class LogLevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }
}

public class JwtBearer
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}
#pragma warning restore CS8618