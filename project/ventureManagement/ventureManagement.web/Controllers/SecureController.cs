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