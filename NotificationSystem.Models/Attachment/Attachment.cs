namespace NotificationSystem.Models.Attachment
{
    public class Attachment
    {
        public long EmailId { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public byte[]? FileStream { get; set; }
    }
}
