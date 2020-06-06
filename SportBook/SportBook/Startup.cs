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
using SportBook.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using FluentValidation.AspNetCore;
using FluentValidation;
using SportBook.ChatHub;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

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
            services.AddControllersWithViews().AddFluentValidation();
            services.AddHttpClient();
            services.AddTransient<IValidator<Event>, EventDateValidator>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // changed to Transient to allow concurrent logins, https://github.com/dotnet/efcore/issues/6488
            services.AddTransient<IServiceSignUp, ServiceSignUp>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var serviceSignUp = serviceProvider.GetService<IServiceSignUp>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options => {
                options.LoginPath = "/Home/Welcome";
            })
            .AddOpenIdConnect("Auth0", options =>
            {
                // Set the authority to your Auth0 domain
                options.Authority = $"https://{Configuration["Authentication:auth0Domain"]}";

                // Configure the Auth0 Client ID and Client Secret
                options.ClientId = Configuration["Authentication:auth0ClientId"];
                options.ClientSecret = Configuration.GetValue<string>("ConnectionStrings:AUTH0_CLIENT_SECRET");
                // Set response type to code
                options.ResponseType = OpenIdConnectResponseType.Code;

                // Configure the scope
                options.Scope.Add("openid");

                // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                options.CallbackPath = new PathString(Configuration["Authentication:auth0RedirectUri"]);
                options.RequireHttpsMetadata = false;
                // Configure the Claims Issuer to be Auth0
                options.ClaimsIssuer = "Auth0";
                // Set the correct name claim type
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "Roles",
                    RoleClaimType = Configuration["Authentication:auth0RoleNameSpace"]
                };
                options.Events = new OpenIdConnectEvents
                {
                    OnTicketReceived = serviceSignUp.CreateOnSignUp,
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
            services.AddSignalR().AddAzureSignalR(Configuration.GetValue<string>("ConnectionStrings:Azure_SignalR"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseCookiePolicy();

            //app.UseStaticFiles();         //replaced by UseFileServer() (for SignalR support)
            app.UseFileServer();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //SecretClientOptions options = new SecretClientOptions()
            //{
            //    Retry =
            //    {
            //        Delay= TimeSpan.FromSeconds(2),
            //        MaxDelay = TimeSpan.FromSeconds(16),
            //        MaxRetries = 5,
            //        Mode = RetryMode.Exponential
            //     }
            //};
            //var client = new SecretClient(new Uri("https://sportbook-secrets.vault.azure.net/"), new DefaultAzureCredential(), options);

            //KeyVaultSecret secret = client.GetSecret("AUTH0-CLIENT-SECRET");

            //string auth0ClientSecret = secret.Value;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<Chat>("/chat");
            });

            var cultureInfo = new CultureInfo("en-US");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

 
    }
}
