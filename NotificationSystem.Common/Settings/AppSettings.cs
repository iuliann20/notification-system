using NotificationSystem.Common.Auth;

namespace NotificationSystem.Common.Settings
{
    public class AppSettings
    {
        public string DatabaseConnection { get; set; }
        public TokenManagement TokenManagement { get; set; }
        public IEnumerable<AppUser> AppUsers { get; set; }

    }
}
