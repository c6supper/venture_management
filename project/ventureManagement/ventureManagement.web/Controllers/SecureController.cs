using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VentureManagement.Web.Controllers
{
    public class SecureController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}