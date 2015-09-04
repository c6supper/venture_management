using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Ext.Net.MVC;
using ventureManagement.web.Models;
using ventureManagement.web.Attributes;

namespace ventureManagement.web.Controllers
{
    [DirectController]
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult TryLogin()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index","Main");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LogOnModel model, string returnUrl)
        {
            // Verify the fields.
            if (ModelState.IsValid)
            {
                // Validate the user login.
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    // Create the authentication ticket.
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    // Redirect to the secure area.
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Secure");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            return View(model);
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
