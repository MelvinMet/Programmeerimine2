using Microsoft.Extensions.Configuration;

namespace KooliProjekt.Application.Authentication.Jwt
{
    public class JwtSettings
    {
        public string Key { get; }
        public string Issuer { get; }
        public string Audience { get; }
        public int ExpireMinutes { get; }

        public JwtSettings(IConfiguration config)
        {
            var section = config.GetSection("Jwt");
            Key = section["Key"];
            Issuer = section["Issuer"];
            Audience = section["Audience"];
            ExpireMinutes = int.TryParse(section["ExpireMinutes"], out var minutes) ? minutes : 60;
        }
    }
}
