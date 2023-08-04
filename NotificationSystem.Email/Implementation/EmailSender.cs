using Microsoft.Extensions.Options;
using NotificationSystem.Common.Settings;
using NotificationSystem.Email.Interface;

namespace NotificationSystem.Email.Implementation
{
    public class EmailSender: IEmailSender
    {
        private readonly AppSettings _appSettings;

        public EmailSender(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<bool> SendBulkEmailsAsync(List<Email>)
    }
}
