using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NotificationSystem.Common.Auth
{
    public class TokenRequest
    {
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required]
        [JsonProperty("username")]
        public string Password { get; set; }
    }
}
