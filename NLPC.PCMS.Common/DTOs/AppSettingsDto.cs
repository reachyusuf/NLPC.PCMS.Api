namespace NLPC.PCMS.Common.DTOs
{
    public class AppSettingsDto
    {
        public string AppName { get; set; }
        
        public RateLimit RateLimit { get; set; }
        public Jwt Jwt { get; set; }
        public Seq Seq { get; set; }
    }

    public class RateLimit
    {
        public bool Enabled { get; set; }
        public int PermitLimit { get; set; }
        public int Window { get; set; }
    }

    public class Jwt
    {
        public string JwtIssuer { get; set; }
        public string JwtSecretKey { get; set; }
        public int JwtTokenExpiredTime { get; set; }
    }

    public class Seq
    {
        public string ServerUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
