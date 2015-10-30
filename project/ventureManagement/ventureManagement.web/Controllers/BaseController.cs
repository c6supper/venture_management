using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VentureManagement.BLL;
using VentureManagement.Models;

namespace VentureManagement.Web.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        // ReSharper disable once InconsistentNaming
        protected User _currentUser
        {
            get
            {
                var userId = (int) System.Web.HttpContext.Current.Session["UserId"];
                var userService = new UserService();
                return userService.Find(userId);
            }
        }

        // ReSharper disable once InconsistentNaming
        protected HashSet<int> _orgHash = null;

        public BaseController()
        {
            _orgHash = System.Web.HttpContext.Current.Session["orgHash"] as HashSet<int>;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
        }

        protected void UpdateCurrentOrgList()
        {
            try
            {
                var userService = new UserService();
                var user = userService.Find(_currentUser.UserName);
                var currentOrgList = new List<int> { user.OrganizationId };
                var orgrService = new OrganizationRelationService();
                currentOrgList.AddRange(orgrService.GetChildrenOrgList(user.OrganizationId));
                System.Web.HttpContext.Current.Session["orgHash"] = new HashSet<int>(currentOrgList);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }
    }
}
