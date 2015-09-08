using System;
using System.Net;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.Models;
using VentureManagement.Web.Attributes;
using VentureManagement.Web.Models;

namespace VentureManagement.Web.Controllers
{
    [DirectController]
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        [OutputCache(Duration = 3600)]
        public ActionResult GetMenuNodes(string node)
        {
            if (node == "Root")
            {
                return this.Content(MenusModel.GetMenuNodes().ToJson());
            }

            return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        [DirectMethod]
        [OutputCache(Duration=86400, VaryByParam="theme")]
        public DirectResult GetThemeUrl(string theme)
        {
            Theme temp = (Theme)Enum.Parse(typeof(Theme), theme);

            this.Session["Ext.Net.Theme"] = temp;

            return this.Direct(temp == Theme.Default ? "Default" : MvcResourceManager.GetThemeUrl(temp));
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
                this.Session["Ext.Net.ScriptMode"] = ScriptMode.Debug;
                this.Session["Ext.Net.SourceFormatting"] = true;
            }
            else
            {
                this.Session["Ext.Net.ScriptMode"] = ScriptMode.Release;
                this.Session["Ext.Net.SourceFormatting"] = false;
            }

            Response.Redirect("");

            return this.Direct();
        }
    }
}
