using MimeKit;
using NotificationSystem.Models.Attachment;
using NotificationSystem.Models.Source;

namespace NotificationSystem.BusinessLogic.Interfaces
{
    public interface IMailBuilder
    {
        IMailBuilder WithSubject(string subject);

        IMailBuilder WithBody(string body);

        IMailBuilder WithAttachments(List<Attachment> attachments);

        IMailBuilder WithRecipients(List<string> recipients);

        IMailBuilder WithCCRecipients(List<string> ccRecipients);

        IMailBuilder WithBCCRecipients(List<string> bccRecipients);

        IMailBuilder WithConfig(SourceModel sourceInfo);

        Task<MimeMessage> Build();
    }
}
