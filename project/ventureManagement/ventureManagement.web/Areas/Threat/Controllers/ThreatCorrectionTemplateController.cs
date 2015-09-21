using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentureManagement.Models;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    public class ThreatCorrectionTemplateController : Controller
    {
        //
        // GET: /Threat/ThreatCorrectionTemplate/
        // ReSharper disable once InconsistentNaming
        private const string _template = "~/Areas/Threat/Content/CorrectionTemplate.xml";
        private ThreatCorrection _correctionTemplate = null;

        public ThreatCorrectionTemplateController()
        {
            _correctionTemplate = ThreatCorrection.Deserialize(System.Web.HttpContext.Current.Server.MapPath(_template));
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
