using LoginFinal.Controllers;
using LoginFinal.DbSeed;
using LoginFinal.Filters;
using LoginFinal.HelpingClasses;
using LoginFinal.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using LoginFinal.DataHub;
using Microsoft.AspNetCore.Http;
using Stripe;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using LoginFinal.BL;

namespace LoginFinal
{
    public class Startup
    {

        public static SqlConnection sq;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
          
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            sq = new SqlConnection(Configuration.GetConnectionString("Default"));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddSingleton<SqlConnection>(sq);

            sq.Open();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")), ServiceLifetime.Transient);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//important to set up/register cookies login
            .AddCookie(options =>
            {
              
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Error/NotFoundPage";
                options.ExpireTimeSpan = TimeSpan.FromDays(9999);
                options.SlidingExpiration = true;
              
            });

            
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<ExceptionFilter>();//important for Custom filters to work properly

            services.AddTransient<GeneralPurpose>(); //Service to get function directly in razor view pages without using constructor injection

            services.AddHttpContextAccessor();//Required to handle httpcontext requests used in general purpose helping class
            
            services.AddSignalR();

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 209715200;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            
            
        }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
            
            StripeConfiguration.ApiKey = Configuration.GetValue<string>("Stripe:SecretKey");
           

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

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { error = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseSignalR();
            app.UseRouting();
           
            //Both classes required to store claims in cookies
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Welcome}/{id?}");
                endpoints.MapHub<ChatHub>("/chat");
            });

            //For Database Seeding
            AppDbInitializer.DbSeed(app);
        }
    }
}
