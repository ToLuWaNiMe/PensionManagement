﻿using Hangfire.Dashboard;

namespace PensionManagement.API
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.Identity.IsAuthenticated; //Allow only authenticated users
        }
    }
}
