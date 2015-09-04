using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using Ext.Net;
using Ext.Net.MVC;
using ventureManagement.web.Areas.Member.Models;
using ventureManagement.web.Attributes;

namespace ventureManagement.web.Controllers
{
    [DirectController]
    public class MainController : System.Web.Mvc.Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Secure");
            }
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        [OutputCache(Duration = 3600)]
        public ActionResult GetExamplesNodes(string node)
        {
            if (node == "Root")
            {
                return this.Content(ExamplesModel.GetExamplesNodes().ToJson());
            }

            return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        [DirectMethod]
        [OutputCache(Duration=86400, VaryByParam="theme")]
        public DirectResult GetThemeUrl(string theme)
        {
            Theme temp = (Theme)Enum.Parse(typeof(Theme), theme);

            this.Session["Ext.Net.Theme"] = temp;

            return this.Direct(temp == Ext.Net.Theme.Default ? "Default" : MvcResourceManager.GetThemeUrl(temp));
        }

        [DirectMethod]
        [OutputCache(Duration = 86400, VaryByParam = "name")]
        public DirectResult GetHashCode(string name)
        {
            return this.Direct(Math.Abs(name.ToLower().GetHashCode()));
        }

        [DirectMethod]
        public DirectResult ChangeScriptMode(bool debug)
        {
            if (debug)
            {
                this.Session["Ext.Net.ScriptMode"] = Ext.Net.ScriptMode.Debug;
                this.Session["Ext.Net.SourceFormatting"] = true;
            }
            else
            {
                this.Session["Ext.Net.ScriptMode"] = Ext.Net.ScriptMode.Release;
                this.Session["Ext.Net.SourceFormatting"] = false;
            }

            Response.Redirect("");

            return this.Direct();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LoginViewModel model, string returnUrl)
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
