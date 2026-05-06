using costa_serena_grand_hotel_FRONTEND.Infrastructure;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace costa_serena_grand_hotel_FRONTEND
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddScoped<AuthPageFilter>();
            
            builder.Services.AddRazorPages()
            .AddMvcOptions(options =>
            {
                options.Filters.AddService<AuthPageFilter>();
            });

            builder.Services.AddHttpClient("costa_serena_grand_hotel_API", c =>
            {

                c.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"]!);

            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler { UseProxy = false }
            )
            .AddHttpMessageHandler<JwtBearerHandler>();

            //SERVICES APIK
            builder.Services.AddScoped<SzobakApi>();
            builder.Services.AddScoped<VendegekApi>();
            builder.Services.AddScoped<FoglalasokApi>();
            builder.Services.AddScoped<ErtekelesekApi>();
            builder.Services.AddScoped<SzobaKategoriakApi>();
            builder.Services.AddScoped<TermekekApi>();
            builder.Services.AddScoped<RendelesekApi>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<AdminApi>();
            builder.Services.AddScoped<ImageStorageService>();

            //JWT AUTH cokkie-khez ezt a kommentent a levi irta
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();


            builder.Services.AddScoped<AuthSession>();
            builder.Services.AddScoped<AuthApi>();
            builder.Services.AddTransient<JwtBearerHandler>();

            var app = builder.Build();

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();
            app.MapControllers();

            app.Run();
        }
    }
}
