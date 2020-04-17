using Microsoft.AspNetCore.Identity;

namespace Model.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

    }
}
