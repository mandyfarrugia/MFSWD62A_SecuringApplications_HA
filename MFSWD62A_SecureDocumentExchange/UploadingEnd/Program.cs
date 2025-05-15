using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace UploadingEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<DocumentExchangeDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<UploadingUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DocumentExchangeDbContext>();

            //Enable external login with a Microsoft account.
            builder.Services.AddAuthentication()
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]!;
                    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]!;
                });

            builder.Services.AddControllersWithViews();

            /* Registers a configuration delegate to modify default settings configured within ASP.NET Core Identity.
             * This can be done to change password rules (such as allowing alphanumeric characters and password length),
             * as well as lockout behaviour (for instance, locking out of a user for an hour after 3 failed password attempts).
             * Such measures can prevent brute-force attacks from malicious users or automated bots. */
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); //Duration of lockout period once user exceeds the maximum allowed failed attempts.
                options.Lockout.MaxFailedAccessAttempts = 5; //How many failed attempts are allowed before lockout.         
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
