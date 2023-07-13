namespace NotificationSystem.Common.Auth
{
    public class AppUser
    {
        public string User { get; set; }
        public string Pass { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
