using NotificationSystem.Common.Auth;

namespace NotificationSystem.BusinessLogic.Interfaces
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(TokenRequest request, out AccessTokenResponse tokenResponse);
    }
}
