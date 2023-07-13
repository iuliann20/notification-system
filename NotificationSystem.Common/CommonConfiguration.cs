using Microsoft.Extensions.DependencyInjection;
using NotificationSystem.Common.Implementation;
using NotificationSystem.Common.Interfaces;

namespace NotificationSystem.Common
{
    public static class CommonConfiguration
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddSingleton<IEncryptionService, EncryptionService>();
        }
    }
}
