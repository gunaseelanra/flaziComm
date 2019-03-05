using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Project.Identity.Web.Data;
using Project.Identity.Web.DomainModel;

namespace Project.Identity.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AuthDbContext>(options => 
             options.UseSqlServer(_configuration.GetConnectionString("AuthDbContext"))
            );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IEmailSender, DummyEmailSender>();

            services.AddAuthentication();
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options => { options.Conventions.AuthorizeFolder("/Account"); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        internal class  DummyEmailSender: IEmailSender
        {
            private readonly ILogger<DummyEmailSender> _logger;

            public DummyEmailSender(ILogger<DummyEmailSender> logger)
            {
                _logger = logger;
            }
            public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                _logger.LogWarning( message:"Dummy implementation is being used!!!");
                return Task.CompletedTask;
            }
        }
    }
}
