using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetUser
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastActivityDate { get; set; }
        public virtual AspNetMembership AspNetMembership { get; set; }
        public virtual AspNetApplication AspNetApplication { get; set; }
    }
}
