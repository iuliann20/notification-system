using MimeKit;
using NotificationSystem.BusinessLogic.Interfaces;
using NotificationSystem.Models.Attachment;
using NotificationSystem.Models.Source;

namespace NotificationSystem.BusinessLogic.Implementation
{
    public class MailBuilder: IMailBuilder
    {
        private string subject;
        private string body;
        private List<Attachment> attachments;
        private List<string> recipients;
        private List<string> ccRecipients;
        private List<string> bccRecipients;
        private SourceModel source;
        private readonly IFileReader _fileReader;

        public MailBuilder(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }
        public IMailBuilder WithSubject(string subject)
        {
            this.subject = subject;

            return this;
        }

        public IMailBuilder WithBody(string body)
        {
            this.body = body;
            return this;
        }

        public IMailBuilder WithAttachments(List<Attachment> attachments)
        {
            this.attachments = attachments;

            return this;
        }

        public IMailBuilder WithRecipients(List<string> recipients)
        {
            this.recipients = recipients;

            return this;
        }
        public IMailBuilder WithCCRecipients(List<string> ccRecipients)
        {
            this.ccRecipients = ccRecipients;

            return this;
        }
        public IMailBuilder WithBCCRecipients(List<string> bccRecipients)
        {
            this.bccRecipients = bccRecipients;

            return this;
        }

        public IMailBuilder WithConfig(SourceModel sourceModel)
        {
            this.source = sourceModel;

            return this;
        }

        public async Task<MimeMessage> Build()
        {
            var sender = new MailboxAddress(source.Name, source.SenderEmail);
            var message = new MimeMessage
            {
                Sender = sender,
                Subject = subject
            };

            message.From.Add(sender);
            message.To.AddRange(FilterRecipients(recipients));

            if (!message.To.Any() && !source.AllowAllRecipients)
            {
                message.To.Add(new MailboxAddress(string.Empty, source.DefaultRecipients));
            }

            message.Cc.AddRange(FilterRecipients(ccRecipients));
            message.Bcc.AddRange(FilterRecipients(bccRecipients));

            var bodyBuilder = new BodyBuilder()
            {
                HtmlBody = body
            };

            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    var filename = $"{attachment.FileName}";
                    //var bytes = await httpClient.GetByteArrayAsync(attachment.Url);
                    var bytes = _fileReader.SetMailAttachemntsAsync(this.source, attachment);
                    bodyBuilder.Attachments.Add(filename, bytes);
                }
            }

            message.Body = bodyBuilder.ToMessageBody();

            Cleanup();

            return message;
        }

        private IEnumerable<MailboxAddress> FilterRecipients(IEnumerable<string> emails)
        {
            var allowedEmails = new List<string>();

            if (emails != null)
            {
                if (!source.AllowAllRecipients)
                {
                    allowedEmails.AddRange(emails.Where(email => source.RecipientsWhiteList.Any(allowedEmail => allowedEmail.ToLowerInvariant().Equals(email.ToLowerInvariant()))).ToList());
                }
                else
                {
                    allowedEmails.AddRange(emails);
                }
            }

            return allowedEmails.Select(email => new MailboxAddress(string.Empty, email)).ToList();
        }

        private void Cleanup()
        {
            subject = null;
            body = null;
            attachments = null;
            recipients = null;
            ccRecipients = null;
            bccRecipients = null;
            source = null;
        }
    }
}
