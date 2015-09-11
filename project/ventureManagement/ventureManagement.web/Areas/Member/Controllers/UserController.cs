using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;

namespace VentureManagement.Web.Areas.Member.Controllers
{
    public class UserController : Controller
    {
        readonly InterfaceUserService _userService = new UserService();
        readonly InterfaceRoleRelationService _userRoleRelationService = new UserRoleRelationService();
        readonly InterfaceUserOrganizationRelationService _userOrgRelationService = new UserOrganizationRelationService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read(StoreRequestParameters parameters)
        {
            return this.Store(UsersPaging(parameters));
        }

        private Paging<User> UsersPaging(StoreRequestParameters parameters)
        {
            return UserPaging(parameters.Start, parameters.Limit, parameters.SimpleSort, parameters.SimpleSortDirection, null);
        }

        private Paging<User> UserPaging(int start, int limit, string sort, SortDirection dir, string filter)
        {
            var pageIndex = start/limit + ((start%limit > 0) ? 1 : 0) + 1;
            var count = 0;
            var users = _userService.FindPageList(pageIndex, limit, out count,0).ToList();

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                users.RemoveAll(user => !user.DisplayName.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                users.Sort(delegate(User x, User y)
                {
                    var direction = dir == SortDirection.DESC ? -1 : 1;

                    var userA = x.GetType().GetProperty(sort).GetValue(x, null);
                    var userB = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(userA, userB) * direction;
                });
            }

            if ((start + limit) > users.Count)
            {
                limit = users.Count - start;
            }

            var rangeUsers = (start < 0 || limit < 0) ? users : users.GetRange(start, limit);

            return new Paging<User>(rangeUsers, users.Count);
        }

    }
}
