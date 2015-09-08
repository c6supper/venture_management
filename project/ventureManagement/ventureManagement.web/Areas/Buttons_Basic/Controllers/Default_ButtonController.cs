using System.Web.Mvc;
using VentureManagement.Models;

namespace VentureManagement.Web.Areas.Buttons_Basic.Controllers
{
    [Authorize(Roles = Role.ROLE_ADMIN)]
    public class Default_ButtonController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
