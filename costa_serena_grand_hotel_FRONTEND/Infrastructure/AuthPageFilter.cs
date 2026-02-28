using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using costa_serena_grand_hotel_FRONTEND.Services;
namespace costa_serena_grand_hotel_FRONTEND.Infrastructure
{
    public class AuthPageFilter : IAsyncPageFilter
    {
        private readonly AuthSession _auth;
        public AuthPageFilter(AuthSession auth) => _auth = auth;
        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        => Task.CompletedTask;
        public Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
        PageHandlerExecutionDelegate next)
        {
            var path = context.HttpContext.Request.Path.Value ?? "/";
            // Bizonyos oldalakat megvédünk
            if (
            //EZEK AZOK AZ OLDALAK, AMIKET CSAK BEJELENTKEZETT FELHASZNÁLÓK LÁTHATNAK
            path.StartsWith("/Spa", StringComparison.OrdinalIgnoreCase) )
{
                if (!_auth.IsSignedIn)
                {
                    context.Result = new RedirectToPageResult("/Account/Login",
                    new { returnUrl = path + context.HttpContext.Request.QueryString });
                    return Task.CompletedTask;
                }
            }
            return next();
        }
    }
}
