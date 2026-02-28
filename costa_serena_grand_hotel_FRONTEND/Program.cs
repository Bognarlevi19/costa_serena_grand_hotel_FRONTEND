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

            /* // Add services to the container.
             builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                 .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

             builder.Services.AddAuthorization(options =>
             {
                 // By default, all incoming requests will be authorized according to the default policy.
                 options.FallbackPolicy = options.DefaultPolicy;
             });
             builder.Services.AddRazorPages()
                 .AddMicrosoftIdentityUI();*/

            builder.Services.AddScoped<AuthPageFilter>();
            // Add services to the container.
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

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
