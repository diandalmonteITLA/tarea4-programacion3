using Microsoft.AspNetCore.Identity;

namespace Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
    }
}
