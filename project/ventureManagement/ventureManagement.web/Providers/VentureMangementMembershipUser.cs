using System;
using System.Web.Security;

namespace VentureManagement.Web.Providers
{
    public class VentureMangementMembershipUser : MembershipUser
    {
        public VentureMangementMembershipUser(long userId, string userName)
            : base(Membership.Provider.Name, userName, userId, null, null, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
        }
    }
}