using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;

namespace CrawlerWebNet6.Services
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //can add some more logic here...
            //return HttpContext.Current.User.Identity.IsAuthenticated;

            //Can use this for NetCore
            return true;// context.GetHttpContext().User.Identity.IsAuthenticated;
        }
    }

}
