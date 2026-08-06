using Application;
using Application.Interfaces;
using Domain.Interfaces;
using Identity;
using Identity.Services;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddIdentityLayerIoc(builder.Configuration);
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddScoped<ListingService>();
            builder.Services.AddTransient<AccountService>();



            var app = builder.Build();

            await app.Services.RunIdentitySeedAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
