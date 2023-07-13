namespace NotificationSystem.Models.Email
{
    public class EmailQueue
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Recipients { get; set; }
        public string CCRecipients { get; set; }
        public string BCCRecipients { get; set; }
        public DateTime CreationDate { get; set; }
        public int? RetryCount { get; set; }
        public DateTime? LastRetryDate { get; set; }
        public DateTime SendingDate { get; set; }
        public int SourceId { get; set; }
        public bool HasAttachment { get; set; }
    }
}
