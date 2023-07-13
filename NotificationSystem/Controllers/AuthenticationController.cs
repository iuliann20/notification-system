using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationSystem.BusinessLogic.Interfaces;
using NotificationSystem.Common.Auth;
using NotificationSystem.Models.Error;

namespace NotificationSystem.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/token")]
        public IActionResult Authenticate([FromBody] TokenRequest tokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AccessTokenResponse token;
            if (_authenticateService.IsAuthenticated(tokenRequest, out token))
            {
                return Ok(token);
            }

            return BadRequest(new ErrorHandler
            {
                Description = "Invalid Request"
            });
        }
    }
}
