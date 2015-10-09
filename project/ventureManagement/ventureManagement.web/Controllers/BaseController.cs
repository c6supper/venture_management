using System;
using System.Collections.Generic;
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
        protected List<int> _currentOrgList = null;

        public BaseController()
        {
            _currentOrgList = System.Web.HttpContext.Current.Session["currentOrgList"] as List<int>;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
        }
    }
}
