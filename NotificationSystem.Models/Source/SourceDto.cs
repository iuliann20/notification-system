namespace NotificationSystem.Models.Source
{
    public class SourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SenderEmail { get; set; }
        public bool AllowAllRecipients { get; set; }
        public string DefaultRecipients { get; set; }
        public string RecipientsWhiteList { get; set; }
        public string EmailServerUser { get; set; }
        public string EmailServerPassword { get; set; }
    }
}
