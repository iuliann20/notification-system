using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotificationSystem.BusinessLogic.Interfaces;
using NotificationSystem.Common.Auth;
using NotificationSystem.Common.Interfaces;
using NotificationSystem.Common.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotificationSystem.BusinessLogic.Implementation
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly AppSettings _appSettings;
        private readonly IEncryptionService _encryptionService;

        public AuthenticateService(IOptions<AppSettings> appSettings, IEncryptionService encryptionService)
        {
            _appSettings = appSettings.Value;
            _encryptionService = encryptionService;
        }

        public bool IsAuthenticated(TokenRequest request, out AccessTokenResponse tokenResponse)
        {
            tokenResponse = null;
            if (!IsValidUser(request.Username, request.Password)) return false;

            var claim = CreateClaim(request);
            if (claim == null) return false;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.TokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _appSettings.TokenManagement.Issuer,
                _appSettings.TokenManagement.Audience,
                claim,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_appSettings.TokenManagement.AccessExpiration),
                signingCredentials: credentials
            );

            tokenResponse = new AccessTokenResponse(jwtToken);
            return true;
        }

        private Claim[] CreateClaim(TokenRequest request)
        {
            Claim[] claim = null;
            var user = _appSettings.AppUsers.FirstOrDefault(user => _encryptionService.Decrypt(user.Pass) == request.Password && _encryptionService.Decrypt(user.User) == request.Username);

            if (user != null)
            {
                var listClaim = new List<Claim>();
                listClaim.Add(new Claim(ClaimTypes.Name, request.Username));
                user.Roles.ToList().ForEach(role => listClaim.Add(new Claim(ClaimTypes.Role, role)));

                return listClaim.ToArray();
            }

            return claim;
        }

        private bool IsValidUser(string username, string password)
        {
            return _appSettings.AppUsers.Any(user => _encryptionService.Decrypt(user.Pass) == password && _encryptionService.Decrypt(user.User) == username);
        }
    }
}