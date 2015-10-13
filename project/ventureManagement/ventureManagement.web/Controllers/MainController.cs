using System;
using System.Collections.Generic;
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
    public class MainController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public List<object> Data
        {
            get
            {
                string[] companies = new string[] 
            { 
                "3m Co",
                "Alcoa Inc",
                "Altria Group Inc",
                "American Express Company",
                "American International Group, Inc.",
                "AT&T Inc.",
                "Boeing Co.",
                "Caterpillar Inc.",
                "Citigroup, Inc.",
                "E.I. du Pont de Nemours and Company",
                "Exxon Mobil Corp",
                "General Electric Company",
                "General Motors Corporation",
                "Hewlett-Packard Co.",
                "Honeywell Intl Inc",
                "Intel Corporation",
                "International Business Machines",
                "Johnson & Johnson",
                "JP Morgan & Chase & Co",
                "McDonald\"s Corporation",
                "Merck & Co., Inc.",
                "Microsoft Corporation",
                "Pfizer Inc",
                "The Coca-Cola Company",
                "The Home Depot, Inc.",
                "The Procter & Gamble Company",
                "United Technologies Corporation",
                "Verizon Communications",
                "Wal-Mart Stores, Inc."
            };

                Random rand = new Random();
                List<object> data = new List<object>(companies.Length);

                for (int i = 0; i < companies.Length; i++)
                {
                    data.Add(new
                    {
                        Company = companies[i],
                        Price = rand.Next(10000) / 100d,
                        Revenue = rand.Next(10000) / 100d,
                        Growth = rand.Next(10000) / 100d,
                        Product = rand.Next(10000) / 100d,
                        Market = rand.Next(10000) / 100d,
                    });
                }

                return data;
            }
        }

        public List<object> RadarData
        {
            get
            {
                return new List<object> 
            { 
                new { Name = "Price", Data = 100 },
                new { Name = "Revenue %", Data = 100 },
                new { Name = "Growth %", Data = 100 },
                new { Name = "Product %", Data = 100 },
                new { Name = "Market %", Data = 100 }
            };
            }
        }

        public ActionResult Home()
        {
            this.ViewData["RadarData"] = RadarData;
            this.ViewData["Data"] = Data;
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
