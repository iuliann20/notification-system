using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationSystem.BusinessLogic.Interfaces;
using NotificationSystem.BusinessLogic.Utils;
using NotificationSystem.Common.Settings;
using NotificationSystem.Common.Utils;
using NotificationSystem.DataAccessLayer;
using NotificationSystem.Models.Email;
using NotificationSystem.Models.Email.Request;
using NotificationSystem.Models.Source;
using System.Net.Http.Headers;
using System.Net.Mail;

namespace NotificationSystem.BusinessLogic.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork _sqluow;
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;
        private readonly IMailBuilder _mailBuilder;

        public EmailService(IUnitOfWork sqluow, ILogger<EmailService> logger, IOptions<AppSettings> appSettings)
        {
            _sqluow = sqluow;
            _logger = logger;
            _appSettings = appSettings.Value;
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

        public async Task<bool> SendBulkEmails(List<EmailToBeSent> emailsToBeSent)
        {
            var processingSuccessful = true;
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    foreach (var emailToBeSent in emailsToBeSent)
                    {
                        await client.ConnectAsync(_appSettings.EmailServerName, _appSettings.EmailServerPort, SecureSocketOptions.StartTlsWhenAvailable);
                        await client.AuthenticateAsync(emailToBeSent.SourceInfo.EmailServerUser, emailToBeSent.SourceInfo.EmailServerPassword);
                        try
                        {
                            var message = await BuildMessage(emailToBeSent.ScheduledEmail, emailToBeSent.SourceInfo);
                            await Retry.Execute(()=> client.SendAsync(message), nameof(client.SendAsync), _appSettings.OperationRetryCount, _appSettings.OperationRetryDelaySeedSeconds);

                            emailToBeSent.ScheduledEmail.Status = (int)Status.Sent;

                        }
                        catch (Exception ex)
                        {
                            processingSuccessful = false;
                            emailToBeSent.ScheduledEmail.Status = (int)Status.Failed;
                        }
                        await client.DisconnectAsync(true);
                    }
                }
            }
            catch (Exception ex)
            {
                processingSuccessful = false;
            }
            return processingSuccessful;
        }

        private async Task<MimeMessage> BuildMessage(ScheduledEmail scheduledEmail, SourceModel sourceModel)
        {
            var builder = _mailBuilder.
                WithSubject(scheduledEmail.Subject)
                .WithBody(scheduledEmail.Body)
                .WithAttachments(scheduledEmail.Attachments.ToList())
                .WithRecipients(scheduledEmail.Recipients.ToList())
                .WithCCRecipients(scheduledEmail.CCRecipients.ToList())
                .WithBCCRecipients(scheduledEmail.BCCRecipients.ToList())
                .WithConfig(sourceModel);
            var email = await builder.Build();

            return email;
        }
    }
}
