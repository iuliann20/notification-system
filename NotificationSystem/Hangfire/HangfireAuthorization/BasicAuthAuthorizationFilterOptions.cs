﻿namespace NotificationSystem.Hangfire.HangfireAuthorization
{
    public class BasicAuthAuthorizationFilterOptions
    {
        public BasicAuthAuthorizationFilterOptions()
        {
            SslRedirect = true;
            RequireSsl = true;
            LoginCaseSensitive = true;
            Users = new BasicAuthAuthorizationUser[] { };
        }

        /// <summary>
        /// Redirects all non-SSL requests to SSL URL
        /// </summary>
        public bool SslRedirect { get; set; }

        /// <summary>
        /// Requires SSL connection to access Hangfire dahsboard. It's strongly recommended to use SSL when you're using basic authentication.
        /// </summary>
        public bool RequireSsl { get; set; }

        /// <summary>
        /// Whether or not login checking is case sensitive.
        /// </summary>
        public bool LoginCaseSensitive { get; set; }

        /// <summary>
        /// Represents users list to access Hangfire dashboard.
        /// </summary>
        public IEnumerable<BasicAuthAuthorizationUser> Users { get; set; }
    }
}
