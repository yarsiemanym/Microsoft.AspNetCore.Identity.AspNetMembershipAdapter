using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetMembershipUser : IdentityUser
    {
        public string PasswordSalt { get; set; }
        public int PasswordFormat { get; set; }
    }
}
