using System.Data;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds
{
    public static class DefaultUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            AppUser user = new()
            {
                Name = "John",
                LastName = "Doe",
                Email = "usuario@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "usuario@email.com"
            };

            if (userManager.Users.All(u => u.Id != user.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(user.Email);
                if (entityUser == null)
                {
                    await userManager.CreateAsync(user, "12Pa$$word!");
                    await userManager.AddToRoleAsync(user, Roles.User.ToString());
                }
            }

            AppUser testDummy = new()
            {
                Name = "Test",
                LastName = "Dummy",
                Email = "testdummy@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "testdummy@email.com"
            };

            if (userManager.Users.All(u => u.Id != user.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(user.Email);
                if (entityUser == null)
                {
                    await userManager.CreateAsync(user, "T3stDummy+p4$$w0rd!");
                    await userManager.AddToRoleAsync(user, Roles.User.ToString());
                }
            }
        }
    }
}

