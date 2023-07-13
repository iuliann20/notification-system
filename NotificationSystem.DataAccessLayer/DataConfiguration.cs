using Microsoft.Extensions.DependencyInjection;

namespace NotificationSystem.DataAccessLayer
{
    public static class DataConfiguration
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
