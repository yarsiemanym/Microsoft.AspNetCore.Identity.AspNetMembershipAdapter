using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetApplication
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string LoweredApplicationName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual ICollection<AspNetMembership> AspNetMemberships { get; set; }

        public AspNetApplication()
        {
            this.AspNetUsers = new HashSet<AspNetUser>();
            this.AspNetMemberships = new HashSet<AspNetMembership>();
        }
    }
}
