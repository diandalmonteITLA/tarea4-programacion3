using System.Data;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds
{
    public static class DefaultUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {

            AppUser testDummy = new()
            {
                Name = "Test",
                LastName = "Dummy",
                Email = "testdummy@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "testdummy@email.com"
            };

            if (userManager.Users.All(u => u.Id != testDummy.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(testDummy.Email);
                if (entityUser == null)
                {
                    await userManager.CreateAsync(testDummy, "T3stDummy+p4$$w0rd!");
                    await userManager.AddToRoleAsync(testDummy, Roles.User.ToString());
                }
            }
        }
    }
}

