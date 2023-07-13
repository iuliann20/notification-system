using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Abstractions;
using NotificationSystem.BusinessLogic.Interfaces;
using NotificationSystem.BusinessLogic.Utils;
using NotificationSystem.DataAccessLayer;
using NotificationSystem.Models.Email;
using NotificationSystem.Models.Email.Request;
using NotificationSystem.Models.Source;

namespace NotificationSystem.BusinessLogic.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork _sqluow;
        private readonly ILogger _logger;

        public EmailService(IUnitOfWork sqluow, ILogger<EmailService> logger)
        {
            _sqluow = sqluow;
            _logger = logger;
        }

        public async Task<long> InsertEmailQueue(ScheduledEmail scheduledEmailRequest)
        {
            long emailId = 0;
            try
            {
                var emailQueue = Mapping.Mapper.Map<EmailQueue>(scheduledEmailRequest);
                emailId = await _sqluow.EmailRepository.Insert(emailQueue);
                if (scheduledEmailRequest.Attachments != null && scheduledEmailRequest.Attachments.Any())
                {
                    foreach (var attachment in scheduledEmailRequest.Attachments)
                    {
                        attachment.EmailId = emailId;
                        await _sqluow.AttachmentRepository.InsertAttachment(attachment);
                    }
                }
                _sqluow.Commit();
            }
            catch (Exception ex)
            {
                emailId = 0;
                _logger.LogError($"Error inserting email for {scheduledEmailRequest.Recipients}, {ex.Message}, {ex.StackTrace}");
            }
            return emailId;
        }

        public async Task<List<EmailToBeSent>> GetEmailsToBeSent(DateTime scheduledSendingDate, int batchSize)
        {
            var emailsToBeSent = new List<EmailToBeSent>();
            try
            {
                var scheduledEmailsDto = await _sqluow.EmailRepository.GetScheduledEmails(scheduledSendingDate, batchSize);

                foreach (var scheduledEmailDto in scheduledEmailsDto)
                {
                    var emailToBeSent = new EmailToBeSent();
                    var scheduledEmail = Mapping.Mapper.Map<ScheduledEmail>(scheduledEmailDto);

                    if (scheduledEmailDto.HasAttachment)
                    {
                        var attachment = await _sqluow.AttachmentRepository.GetAttachmentsByEmailId(scheduledEmailDto.Id);
                        scheduledEmail.Attachments = attachment;
                    }

                    emailToBeSent.ScheduledEmail = scheduledEmail;

                    var sourceDto = await _sqluow.SourceRepository.GetSourceById(scheduledEmailDto.Id);
                    emailToBeSent.SourceInfo = Mapping.Mapper.Map<SourceModel>(sourceDto);

                    emailsToBeSent.Add(emailToBeSent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting email for {scheduledSendingDate}, {ex.Message}, {ex.StackTrace}");
            }

            return emailsToBeSent;
        }
    }
}
