using Identity.Context;
using Identity.Entities;
using Identity.Seeds;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity
{
    public static class ServicesRegistration
    {
        public static void AddIdentityLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            GeneralConfiguration(services, config);

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opt.Lockout.MaxFailedAccessAttempts = 3;

                opt.User.RequireUniqueEmail = true;
                // opt.SignIn.RequireConfirmedEmail = true;
            });


            services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(8);
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                opt.SlidingExpiration = true; //Las cookies expiran despues de 10 minutos de inactividad. Se redirecciona al usuario al login
                opt.LoginPath = "/Login";
                opt.AccessDeniedPath = "/Login/AccessDenied";
            });

        }

        public static async Task RunIdentitySeedAsync(this IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var servicesProvider = scope.ServiceProvider;

                var userManager = servicesProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = servicesProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await DefaultRoles.SeedAsync(roleManager);
                await DefaultUser.SeedAsync(userManager);
            }
        }

        private static void GeneralConfiguration(IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<IdentityContext>(
                (serviceProvider, opt) =>
                {
                    opt.UseSqlServer(connectionString,
                    m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                },
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Scoped
            );
        }
    }
}

