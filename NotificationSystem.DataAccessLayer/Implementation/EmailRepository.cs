using Dapper;
using NotificationSystem.DataAccessLayer.Interfaces;
using NotificationSystem.Models.Email;
using System.Data;

namespace NotificationSystem.DataAccessLayer.Implementation
{
    public class EmailRepository : RepositoryBase, IEmailRepository
    {
        private const string EMAIL_QUEUE_INSERT = "EmailQueue_Insert";
        private const string EMAIL_QUEUE_GET = "EmailQueue_Get";
        public EmailRepository(IDbTransaction transaction, IDbConnection connection) : base(transaction, connection)
        {
        }

        public async Task<long> Insert(EmailQueue emailQueue)
        {
            var parameters = new DynamicParameters(new
            {
                emailQueue.Subject,
                emailQueue.Body,
                emailQueue.Recipients,
                emailQueue.CCRecipients,
                emailQueue.BCCRecipients,
                emailQueue.CreationDate,
                emailQueue.RetryCount,
                emailQueue.LastRetryDate,
                emailQueue.SendingDate,
                emailQueue.SourceId,
                emailQueue.HasAttachment
            });

            var id = await Connection.QueryAsync<int>(
                sql: EMAIL_QUEUE_INSERT,
                param: parameters,
                commandType: CommandType.StoredProcedure,
                transaction: Transaction
                );
            return id.FirstOrDefault();
        }

        public async Task<IEnumerable<EmailQueue>> GetScheduledEmails(DateTime scheduledSendingDate, int batch)
        {
            var parameters = new DynamicParameters(new
            {
                Top = batch,
                ScheduledSendingDate = scheduledSendingDate
            });

            var emails = await Connection.QueryAsync<EmailQueue>(
                sql: EMAIL_QUEUE_GET,
                param: parameters,
                commandType: CommandType.StoredProcedure,
                transaction: Transaction
                );

            return emails;
        }
    }
}
