using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VentureManagement.Models;

namespace VentureManagement.Web.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        // ReSharper disable once InconsistentNaming
        protected User _currentUser = null;
        // ReSharper disable once InconsistentNaming
        protected List<int> _currentOrgList = null;

        public BaseController()
        {
            _currentUser = System.Web.HttpContext.Current.Session["User"] as User;
            _currentOrgList = System.Web.HttpContext.Current.Session["currentOrgList"] as List<int>;

            var cultureName = "zh-CN";

            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
            ViewData["lang"] = cultureName;
        }
    }
}
