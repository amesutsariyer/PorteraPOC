using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PorteraPOC.Business;
using PorteraPOC.Business.Manager;
using PorteraPOC.Business.Service;
using PorteraPOC.DataAccess.Data;
using PorteraPOC.DataAccess.Interface;
using PorteraPOC.DataAccess.UnitOfWork;
using PorteraPOC.Dto.Validations;



namespace PorteraPOC.Web.IDS
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IHostingEnvironment environment)
        {
            Configuration = new ConfigurationBuilder()
              .SetBasePath(environment.ContentRootPath)
              //.AddJsonFile("appsettings.json", false, true)
              .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
              .AddEnvironmentVariables()
              .Build();
            PublicConfiguration = Configuration;
        }

        //public IConfiguration Configuration { get; }
        public static IConfiguration PublicConfiguration { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PorteraDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));



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

               routes.MapRoute(
                    name: "RedirectToIds",
                    template: "/{param?}",
                    defaults: new { controller = "Home", action = "RedirectToIds" },
                    constraints: null,
                    dataTokens: new { actionName = "" });
            });
        }
    }
}
