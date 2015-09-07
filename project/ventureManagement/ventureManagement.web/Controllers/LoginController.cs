using System.Web.Mvc;
using System.Web.Security;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Controllers
{
    [DirectController(AreaName = "Member", GenerateProxyForOtherControllers = false, IDMode = DirectMethodProxyIDMode.None)]
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if(!Request.IsAuthenticated)
                return View();

            return RedirectToAction("Index","Main");
        }

        [AllowAnonymous]
        public ActionResult Login(string txtUsername, string txtPassword, string returnUrl)
        {
            // Validate the user login.
            if (Membership.ValidateUser(txtUsername, txtPassword))
            {
                // Create the authentication ticket.
                FormsAuthentication.SetAuthCookie(txtUsername, false);

                // Redirect to the secure area.
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToRoute("Default", new { Controller = "Main", Action = "Index" }); ;
                }
            }
            else
            {
                X.Msg.Alert("提示", "用户名或密码错误,请重新输入").Show();
            }

            return this.Direct();
        }

        public ActionResult LogOff()
        {
            // Delete the user details from cache.
            System.Web.HttpContext.Current.Cache.Remove(User.Identity.Name);

            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}
