using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Silbernetz.Actions;
using Silbernetz.Controllers.SignalHub;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Silbernetz.Database;
using Silbernetz.Models.Conf;

namespace Silbernetz
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
            services.AddOptions();
            services.Configure<InoplaConf>(Configuration.GetSection(nameof(InoplaConf)));
            services.AddSingleton<InoplaClient>();
            services.AddSingleton<AnrufSafe>();
            AzureActiveDirectory AADConfig = Configuration.GetSection(nameof(AzureActiveDirectory)).Get<AzureActiveDirectory>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.Authority = AADConfig.Issuer;

                    options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        LifetimeValidator = (before, expires, token, parameters) => expires > DateTime.UtcNow,
                        ValidateAudience = true,
                        ValidAudiences = new string[] { AADConfig.ClientId, AADConfig.TenantId },
                        ValidateIssuer = true,
                        ValidIssuer = AADConfig.Issuer + "/v2.0",
                        ValidateActor = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AADConfig.ClientKey)),

                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            if (!string.IsNullOrEmpty(accessToken) &&
                                (context.HttpContext.WebSockets.IsWebSocketRequest || context.Request.Headers["Accept"] == "text/event-stream"))
                            {
                                context.Token = context.Request.Query["access_token"];
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSignalR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AnrufSafe database)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST")
                        .AllowCredentials();
                });
                app.UseDeveloperExceptionPage();
                // app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseCors("FrontEnd");
            app.UseWebSockets();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<SignalRHub>("/Hub");
            });
            //Initialisierung beim Starten, zu diesem Zeitpunkt wird die Anruf Statistik über die api geholt, und dies kann dauern.
            database.Init();
        }
    }
}
