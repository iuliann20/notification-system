using Microsoft.Extensions.DependencyInjection;
using NotificationSystem.BusinessLogic.Implementation;
using NotificationSystem.BusinessLogic.Interfaces;

namespace NotificationSystem.BusinessLogic
{
    public static class BusinessConfiguration
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<IAuthenticateService, AuthenticateService>();
        }
    }
}
