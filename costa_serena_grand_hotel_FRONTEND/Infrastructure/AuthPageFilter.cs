using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace costa_serena_grand_hotel_FRONTEND.Infrastructure
{
    public class AuthPageFilter : IAsyncPageFilter
    {
        private readonly AuthSession _authSession;

        public AuthPageFilter(AuthSession authSession)
        {
            _authSession = authSession;
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var path = context.HttpContext.Request.Path.Value ?? string.Empty;

            var loginRequired =
                path.StartsWith("/Foglalasaim", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/Vendeg", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/Shop/Checkout", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase);

            if (loginRequired && !_authSession.IsSignedIn)
            {
                context.Result = new RedirectToPageResult("/Account/Login");
                return Task.CompletedTask;
            }

            if (path.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase) && !_authSession.IsInRole("Admin"))
            {
                context.Result = new RedirectToPageResult("/Errors/Forbidden");
                return Task.CompletedTask;
            }

            return next();
        }
    }
}