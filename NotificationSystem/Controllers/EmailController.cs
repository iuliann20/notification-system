using Microsoft.AspNetCore.Mvc;
using NotificationSystem.BusinessLogic.Interfaces;
using NotificationSystem.Models.Email.Request;
using System.Web.Http;

namespace NotificationSystem.Controllers
{
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("email/InsertEmail")]
        public async Task<IActionResult> InsertEmail(ScheduledEmail email)
        {
            var sourceIdEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "SourceIdEmail")?.Value;

            if (string.IsNullOrEmpty(sourceIdEmail))
            {
                return BadRequest();
            }

            int.TryParse(sourceIdEmail, out int sourceId);
            email.SourceId = sourceId;
            var id = await _emailService.InsertEmailQueue(email);

            if (id != 0)
            {
                return Created(string.Empty, id);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
