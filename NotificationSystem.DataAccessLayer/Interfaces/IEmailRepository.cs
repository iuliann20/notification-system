using NotificationSystem.Models.Email;

namespace NotificationSystem.DataAccessLayer.Interfaces
{
    public interface IEmailRepository
    {
        Task<long> Insert(EmailQueue emailQueue);
        Task<IEnumerable<EmailQueue>> GetScheduledEmails(DateTime scheduledSendingDate, int batch);
    }
}
