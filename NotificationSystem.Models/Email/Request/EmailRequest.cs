using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace NotificationSystem.Models.Email.Request
{
    public class EmailRequest
    {
        [SwaggerSchema("Auto-generated identifier", ReadOnly =true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Body { get; set; }

        public List<string> Recipients { get; set; }
        public List<string> CCRecipients { get; set; }
        public List<string> BCCRecipients { get; set; }
        public DateTime CreationDate { get; set; }
        public int? RetryCount { get; set; }
        public DateTime? LastRetryDate { get; set; }
        public DateTime SendingDatet { get; set; }
        public int SourceId { get; set; }
        public bool HasAttachment { get; set; }
        public IEnumerable<Attachment.Attachment> Attachments { get; set; }
    }
}
