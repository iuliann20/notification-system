using System.Collections.Generic;

namespace NotificationSystem.Models.Email.Request
{
    public class ScheduledEmail
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> Recipients { get; set; }
        public IEnumerable<string> CCRecipients { get; set; }
        public IEnumerable<string> BCCRecipients { get; set; }
        public DateTime CreationDate { get; set; }
        public int? RetryCount { get; set; }
        public DateTime? LastRetryDate { get; set; }
        public DateTime SendingDatet { get; set; }
        public int SourceId { get; set; }
        public bool HasAttachment { get; set; }
        public IEnumerable<NotificationSystem.Models.Attachment.Attachment> Attachments { get; set; }
    }
}
