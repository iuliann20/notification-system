using NotificationSystem.DataAccessLayer.Interfaces;

namespace NotificationSystem.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IAttachmentRepository AttachmentRepository { get; }
        ISourceRepository SourceRepository { get; }
        IEmailRepository EmailRepository { get; }
        void Commit();
    }
}
