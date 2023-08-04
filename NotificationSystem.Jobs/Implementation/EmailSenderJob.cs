using Microsoft.Extensions.Options;
using NotificationSystem.BusinessLogic.Interfaces;
using NotificationSystem.Common.Settings;

namespace NotificationSystem.Jobs.Implementation
{
    public class EmailSenderJob
    {
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;

        public EmailSenderJob(IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _emailService = emailService;
            _appSettings = appSettings.Value;
        }

        public async Task ProcessEmails(DateTime currentDate)
        {
            var processedSuccessful = true;
            try
            {
                var emails = await _emailService.GetEmailsToBeSent(currentDate, _appSettings.BatchSize);

                if(emails!= null && emails.Any())
                {
                    //processedSuccessful = await 
                }
            }
            catch (Exception ex)
            {
                processedSuccessful = false;
            }
        }
    }
}
