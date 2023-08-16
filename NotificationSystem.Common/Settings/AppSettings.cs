using NotificationSystem.Common.Auth;

namespace NotificationSystem.Common.Settings
{
    public class AppSettings
    {
        public string DatabaseConnection { get; set; }
        public TokenManagement TokenManagement { get; set; }
        public IEnumerable<AppUser> AppUsers { get; set; }
        public int BatchSize { get; set; }
        public string EmailServerName { get; set; }
        public int EmailServerPort { get; set; }
        public string AttachmentPathFile { get; set; }
        public string HangfireUserLogin { get; set; }
        public string HangfireUserPassword { get; set; }
        public int OperationRetryCount { get; set; }
        public int OperationRetryDelaySeedSeconds { get; set; }
    }
}
