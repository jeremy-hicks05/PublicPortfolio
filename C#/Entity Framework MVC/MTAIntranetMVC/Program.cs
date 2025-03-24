

namespace MTAIntranet.MVC
{
    using Microsoft.AspNetCore.Authentication.Negotiate;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using MTAIntranet.MVC.Utility;
    //using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
    using MTAIntranet.Shared;
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var MTAconnectionString = builder.Configuration
                .GetConnectionString("MTAConnection") ??
                throw new InvalidOperationException(
                    "Connection string 'MTAConnection' not found");

            var FuelmasterConnectionString = builder.Configuration
                .GetConnectionString("FuelmasterConnection") ??
                throw new InvalidOperationException(
                    "Connection string 'FuelmasterConnection' not found");

            var EAMProdConnectionString = builder.Configuration
                .GetConnectionString("EAMProdConnection") ??
                throw new InvalidOperationException(
                    "Connection string 'EAMProdConnection' not found");

            // Add services to the container
            builder.Services.AddDbContext<EAMProdContext>(options =>
            //options.UseSqlServer(EAMconnectionString));
            options.UseSqlServer(EAMProdConnectionString));

            builder.Services.AddDbContext<FuelmasterContext>(options =>
                options.UseSqlServer(FuelmasterConnectionString));
                //options.UseSqlServer(EAMconnectionString));

            builder.Services.AddDbContext<MTAIntranetContext>(options =>
                options.UseSqlServer(MTAconnectionString));
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() // enable role management
                .AddEntityFrameworkStores<MTAIntranetContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(
                NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate()
            //.AddCookie(options =>
            //    options.AccessDeniedPath = $"/Views/Access/Denied");
            //    builder.Services.AddAuthorization(options =>
            //        {
            //            options.FallbackPolicy = options.DefaultPolicy;
            //        })
            ;
            builder.Services.AddRazorPages();

            // edit access denied path
            //builder.Services.ConfigureApplicationCookie(options =>
            //    options.AccessDeniedPath = $"/Views/Access/Denied"
            //);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // edit status code page middleware
            //app.UseStatusCodePages(async context =>
            //{
            //    if(context.HttpContext.Response.StatusCode == 403)
            //    {
            //        return NotFoundResult("")
            //    }
            //});

            app.Run();
        }
    }
}