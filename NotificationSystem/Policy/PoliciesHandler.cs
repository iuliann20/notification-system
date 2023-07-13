using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace NotificationSystem.Policy
{
    public class PoliciesHandler
    {
        public static void SetPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("insertEmail", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireRole("insertemail");
            });
        }
    }
}
