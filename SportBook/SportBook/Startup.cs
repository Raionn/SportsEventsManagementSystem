using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SportBook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("LOCAL_CONNECTION");

            if (connectionString == null)
                connectionString = Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

            services.AddDbContext<SportbookDatabaseContext>(options =>
                               options.UseSqlServer(connectionString));
            services.AddControllersWithViews();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie()
            .AddOpenIdConnect("Auth0", options =>
            {
                // Set the authority to your Auth0 domain
                options.Authority = $"https://{Configuration["Authentication:auth0Domain"]}";

                // Configure the Auth0 Client ID and Client Secret
                options.ClientId = Configuration["Authentication:auth0ClientId"];
                options.ClientSecret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET");

                // Set response type to code
                options.ResponseType = OpenIdConnectResponseType.Code;

                // Configure the scope
                options.Scope.Add("openid");

                // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                options.CallbackPath = new PathString("/General/Profile");
                options.RequireHttpsMetadata = false;
                // Configure the Claims Issuer to be Auth0
                options.ClaimsIssuer = "Auth0";

                options.Events = new OpenIdConnectEvents
                {
                    OnTicketReceived = (context) =>
                    {
                        var request = context.Request;
                        var homeurl = request.Scheme + "://" + request.Host + request.PathBase + "/";
                        context.Response.Redirect(homeurl);
                        return Task.CompletedTask;
                    },
                    // handle the logout redirection
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                   {
                       var logoutUri = $"https://{Configuration["Authentication:auth0Domain"]}/v2/logout?client_id={Configuration["Authentication:auth0ClientId"]}";

                       var postLogoutUri = context.Properties.RedirectUri;
                       if (!string.IsNullOrEmpty(postLogoutUri))
                       {
                           if (postLogoutUri.StartsWith("/"))
                           {
                       // transform to absolute
                       var request = context.Request;
                               postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                           }
                           logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                       }

                       context.Response.Redirect(logoutUri);
                       context.HandleResponse();

                       return Task.CompletedTask;
                   }
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string auth0Domain = Configuration["Authentication:auth0Domain"];
            string auth0ClientId = Configuration["Authentication:auth0ClientId"];
            string auth0ClientSecret = Configuration["Authentication:auth0ClientSecret"];
            string auth0RedirectUri = Configuration["Authentication:auth0RedirectUri"];
            string auth0PostLogoutRedirectUri = Configuration["Authentication:auth0PostLogoutRedirectUri"];

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
