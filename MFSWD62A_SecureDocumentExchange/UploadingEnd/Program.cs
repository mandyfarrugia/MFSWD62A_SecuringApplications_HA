using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UploadingEnd
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<DocumentExchangeDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            /* Registers a configuration delegate to modify default settings configured within ASP.NET Core Identity.
             * This can be done to change password rules (such as allowing alphanumeric characters and password length),
             * as well as lockout behaviour (for instance, locking out of a user for an hour after 3 failed password attempts).
             * Such measures can prevent brute-force attacks from malicious users or automated bots. */
            builder.Services.AddIdentity<UploadingUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true; //Whether the user must confirm their email address before they can log in.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); //Duration of lockout period once user exceeds the maximum allowed failed attempts.
                options.Lockout.MaxFailedAccessAttempts = 5; //How many failed attempts are allowed before lockout.         
                options.Password.RequireNonAlphanumeric = true; //Whether password should comprise of characters that are neither numbers (0-9) nor letters (a-z/A-Z), instead including symbols, punctuation marks, and special characters.
                options.Password.RequireUppercase = true; //Whether password must at least include one uppercase character.
                options.Password.RequiredLength = 8; //Used to specify the minimum length of a password.
            })
                .AddDefaultUI()
                .AddEntityFrameworkStores<DocumentExchangeDbContext>()
                .AddDefaultTokenProviders();

            //Enable external login with a Microsoft account.
            builder.Services.AddAuthentication()
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]!;
                    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]!;
                });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using(IServiceScope? scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRolesAsync(services);
            }

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

        public static async Task SeedRolesAsync(IServiceProvider services) 
        {
            RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = new string[] { "Client", "Lawyer" };

            foreach (string role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
