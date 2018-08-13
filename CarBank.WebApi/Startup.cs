using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CarBank.WebApi.Data;
using CarBank.WebApi.Models;
using CarBank.WebApi.Services;
using CarBank.WebApi.Filters;
using CarBank.WebApi.Services.JWT;
using System;

namespace CarBank.WebApi
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity Options
            services.ConfigureIdentityOptions();
            services.ConfigureJwtOptions(Configuration);

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IJwtFactory, JwtFactory>();
            // add CORS
            services.AddCors();

            services.AddMvc(options => options.Filters.Add(typeof(ActionValidationFilter)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(builder =>
                        builder.WithOrigins("http://localhost:3000")
                               .AllowAnyHeader().AllowAnyMethod().SetPreflightMaxAge(TimeSpan.FromSeconds(2520))
                        );

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
