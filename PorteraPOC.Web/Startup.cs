using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PorteraPOC.Business;
using PorteraPOC.Business.Manager;
using PorteraPOC.Business.Service;
using PorteraPOC.DataAccess;
using PorteraPOC.DataAccess.Data;
using PorteraPOC.DataAccess.Interface;
using PorteraPOC.DataAccess.UnitOfWork;
using PorteraPOC.Dto.Validations;
using PorteraPOC.Entity;
using Serilog;

namespace PorteraPOC.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Serilog.Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(Configuration)
           .CreateLogger();
            PublicConfiguration = Configuration;

        }

        public IConfiguration Configuration { get; }
        public static IConfiguration PublicConfiguration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<PorteraDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddTransient<IPilotService, PilotManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMvc()
  .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PilotDtoValidation>());
            // Auto Mapper

            Mapper.Initialize(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
