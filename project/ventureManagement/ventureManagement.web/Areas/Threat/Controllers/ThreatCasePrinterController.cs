using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentureManagement.Models;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    public class ThreatCasePrinterController : ThreatBaseController
    {
        //
        // GET: /Threat/ThreatCasePrinter/

        public ActionResult Index(int threatCaseId)
        {
            var threatCase = _threatCaseService.Find(threatCaseId);
            return View(threatCase);
        }
    }
}
