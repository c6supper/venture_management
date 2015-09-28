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

        public ActionResult Index(int? threadCaseId)
        {
            if (threadCaseId == null)
                threadCaseId = 1;

            var threatCase = _threatCaseService.Find((int)threadCaseId);
            return View(threatCase);
        }
    }
}
