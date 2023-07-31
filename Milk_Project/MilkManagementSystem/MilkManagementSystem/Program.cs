using MilkManagementSystem.DbSeed;
using MilkManagementSystem.Filters;
using MilkManagementSystem.HelpingClasses;
using MilkManagementSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

    var builder = WebApplication.CreateBuilder(args);

    var sq = new SqlConnection(builder.Configuration.GetConnectionString("Default"));

    // Add services to the container.
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")),ServiceLifetime.Transient);

    builder.Services.AddSingleton<SqlConnection>(sq); //SqlConnection Dependency Resolver

    //sq.Open();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//important to set up/register cookies login
    .AddCookie(options =>
    {

        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Error/NotFoundPage";
        options.ExpireTimeSpan = TimeSpan.FromDays(9999);
        options.SlidingExpiration = true;

    });


    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.CheckConsentNeeded = context => false;

        options.MinimumSameSitePolicy = SameSiteMode.None;
    }); //Cookie Policy Set


    builder.Services.AddScoped<ExceptionFilter>();//important for Custom filters to work properly

    builder.Services.AddTransient<GeneralPurpose>(); //Service to get function directly in razor view pages without using constructor injection

    builder.Services.AddHttpContextAccessor();//Required to handle httpcontext requests used in general purpose helping class

    

    builder.Services.Configure<FormOptions>(x =>
    {
        x.MultipartBodyLengthLimit = 209715200;
    }); //Form Body Length Limit //for file uploading


    builder.Services.Configure<IISServerOptions>(options =>
    {
        options.MaxRequestBodySize = int.MaxValue;
    });


    var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseAuthentication();

app.UseCookiePolicy();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

//For Database Seeding
AppDbInitializer.DbSeed(app);

app.Run();
