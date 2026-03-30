using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using costa_serena_grand_hotel_FRONTEND.Services;

namespace costa_serena_grand_hotel_FRONTEND.Infrastructure
{
    public class AuthPageFilter : IAsyncPageFilter
    {
        private readonly AuthSession _auth;

        public AuthPageFilter(AuthSession auth)
        {
            _auth = auth;
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
            => Task.CompletedTask;

        public Task OnPageHandlerExecutionAsync(
            PageHandlerExecutingContext context,
            PageHandlerExecutionDelegate next)
        {
            var path = context.HttpContext.Request.Path.Value ?? "/";

            // IDE majd később betehetsz olyan oldalakat,
            // amiket tényleg csak bejelentkezett felhasználó láthat.
            var bejelentkezestIgenyloOldal =
                path.StartsWith("/Foglalasaim", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/Vendeg", StringComparison.OrdinalIgnoreCase);

            if (bejelentkezestIgenyloOldal && !_auth.IsSignedIn)
            {
                context.Result = new RedirectToPageResult(
                    "/Account/Login",
                    new { returnUrl = path + context.HttpContext.Request.QueryString });
                return Task.CompletedTask;
            }

            return next();
        }
    }
}