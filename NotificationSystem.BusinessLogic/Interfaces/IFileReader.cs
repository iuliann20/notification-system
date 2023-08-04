using NotificationSystem.Models.Attachment;
using NotificationSystem.Models.Source;

namespace NotificationSystem.BusinessLogic.Interfaces
{
    public interface IFileReader
    {
        byte[] SetMailAttachemntsAsync(SourceModel sourceModel, Attachment attachment);
    }
}
