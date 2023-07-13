using System.IdentityModel.Tokens.Jwt;

namespace NotificationSystem.Common.Auth
{
    public class AccessTokenResponse
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public double ExpiresInSeconds { get; set; }
        public AccessTokenResponse(JwtSecurityToken securityToken)
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            TokenType = "Bearer";
            ExpiresInSeconds = Math.Truncate((securityToken.ValidTo - DateTime.UtcNow).TotalSeconds);
        }
    }
}
