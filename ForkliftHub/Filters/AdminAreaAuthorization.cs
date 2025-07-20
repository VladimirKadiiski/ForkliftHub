using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForkliftHub.Filters
{
    public class AdminAreaAuthorization : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var area = context.RouteData.Values["area"]?.ToString();

            if (string.Equals(area, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                var user = context.HttpContext.User;

                if (!user.Identity?.IsAuthenticated ?? false)
                {
                    context.Result = new RedirectToPageResult("/Account/Login", new { area = "Identity" });

                }
                else if (!user.IsInRole("Admin"))
                {
                    context.Result = new RedirectToPageResult("/Account/AccessDenied", new { area = "Identity" });

                }
            }
        }
    }
}
