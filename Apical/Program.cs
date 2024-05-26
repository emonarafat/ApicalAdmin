using Apical.Areas.Identity;
using Apical.Data;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Apical;


public static class AppStrings
{
    public static string Title { get; set; } = "Apical";
    public static string Version { get; set; } = GetAssemblyVersion();
	public static string GetAssemblyVersion()
	{
		return typeof(AppStrings).Assembly.GetName().Version!.ToString();
	}
}

public class Program
{


    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();


        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {

            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

        }).AddRoles<IdentityRole>()
          .AddRoleManager<RoleManager<IdentityRole>>()
          .AddEntityFrameworkStores<ApplicationDbContext>();



        builder.Services.AddRazorPages();

        builder.Services.AddServerSideBlazor(options =>
        {
            // Enable detailed error handling
            // Turn this off for production builds
            options.DetailedErrors = true;
        });


        builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}