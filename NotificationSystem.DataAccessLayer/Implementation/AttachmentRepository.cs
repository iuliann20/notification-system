using Dapper;
using NotificationSystem.DataAccessLayer.Interfaces;
using NotificationSystem.Models.Attachment;
using System.Data;

namespace NotificationSystem.DataAccessLayer.Implementation
{
    public class AttachmentRepository: RepositoryBase, IAttachmentRepository
    {
        private const string ATTACHMENT_INSERT = "Attachment_Insert";
        private const string ATTACHMENT_GET_BY_EMAILID = "Attachment_Get_ByEmailId";
        private const string ATTACHMENT_ARCHIVE_INSERT = "AttachmentArchive_Insert";

        public AttachmentRepository(IDbTransaction transaction, IDbConnection connection) : base(transaction, connection)
        {
        }

        public async Task InsertAttachment(Attachment attachment)
        {
            var parameters = new DynamicParameters(new
            {
                EmailId = attachment.EmailId,
                Path= attachment.Path,
                FileName = attachment.FileName
            });

            await Connection.ExecuteAsync(
                sql: ATTACHMENT_INSERT,
                param: parameters,
                commandType: CommandType.StoredProcedure,
                transaction : Transaction
                );
        }

        public async Task<IEnumerable<Attachment>> GetAttachmentsByEmailId(long emailId)
        {
            var parameters = new DynamicParameters(new
            {
                EmailId = emailId
            });

            var attachments = await Connection.QueryAsync<Attachment>(
                sql: ATTACHMENT_GET_BY_EMAILID,
                param: parameters,
                commandType : CommandType.StoredProcedure,
                transaction : Transaction
                );

            return attachments;
        }
    }
}
