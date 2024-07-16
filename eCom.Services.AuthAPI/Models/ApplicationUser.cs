using Microsoft.AspNetCore.Identity;

namespace eCom.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }    
    }
}
