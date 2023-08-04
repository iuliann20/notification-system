using NotificationSystem.DataAccessLayer;
using NotificationSystem.Models.Email;
using NotificationSystem.Models.Email.Request;

namespace NotificationSystem.BusinessLogic.Interfaces
{
    public interface IEmailService
    {
        Task<long> InsertEmailQueue(ScheduledEmail scheduledEmailRequest);
        Task<List<EmailToBeSent>> GetEmailsToBeSent(DateTime scheduledSendingDate, int batchSize);
    }
}
