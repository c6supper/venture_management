using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ventureManagement.web.Providers
{
    public class VentureMangementMembershipUser : MembershipUser
    {
        public VentureMangementMembershipUser(long userId, string userName)
            : base(Membership.Provider.Name, userName, userId, null, null, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
        }
    }
}