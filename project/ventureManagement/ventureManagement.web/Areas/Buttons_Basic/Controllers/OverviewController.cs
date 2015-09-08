using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;

namespace VentureManagement.Web.Areas.Buttons_Basic.Controllers
{
    public class OverviewController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrators")]
		public ActionResult Button_Click(string Item)
		{
			X.Msg.Alert("DirectEvent", string.Format("Item - {0}",  Item)).Show();
			return this.Direct();
		}
    }
}
