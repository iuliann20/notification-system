using NotificationSystem.Models.Email.Request;
using NotificationSystem.Models.Source;

namespace NotificationSystem.Models.Email
{
    public class EmailToBeSent
    {
        public ScheduledEmail ScheduledEmail { get; set; }
        public SourceModel SourceInfo { get; set; }
    }
}
