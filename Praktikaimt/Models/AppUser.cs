using Microsoft.AspNetCore.Identity;

namespace Praktikaimt.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }

    }
}
