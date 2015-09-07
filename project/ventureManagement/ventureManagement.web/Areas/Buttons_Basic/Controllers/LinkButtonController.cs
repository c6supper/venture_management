using System;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;

namespace VentureManagement.Web.Areas.Buttons_Basic.Controllers
{
    public class LinkButtonController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Button_Click()
		{
			X.Msg.Alert("Server Time", DateTime.Now.ToLongTimeString()).Show();
			return this.Direct();
		}
    }
}
