using NotificationSystem.Models.Attachment;

namespace NotificationSystem.DataAccessLayer.Interfaces
{
    public interface IAttachmentRepository
    {
        Task InsertAttachment(Attachment attachment);
        Task<IEnumerable<Attachment>> GetAttachmentsByEmailId(long emailId);
    }
}
