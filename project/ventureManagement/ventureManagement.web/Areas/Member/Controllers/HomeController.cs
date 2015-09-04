using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ventureManagement.web.Areas.Member.Controllers
{
    /// <summary>
    /// 会员主页控制器
    /// <remarks>
    /// 创建：2014.02.13
    /// </remarks>
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}