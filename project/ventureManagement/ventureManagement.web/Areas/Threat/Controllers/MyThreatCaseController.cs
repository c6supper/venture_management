using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    public class MyThreatCaseController : ThreatBaseController
    {
        //
        // GET: /Threat/MyThreatCase/

        public ActionResult Index()
        {
            var threatCases =
                _threatCaseService.FindList(
                    t => t.ThreatCaseOwnerId == _currentUser.UserId || t.ThreatCaseConfirmerId == _currentUser.UserId ||
                         t.ThreatCaseReporterId == _currentUser.UserId || t.ThreatCaseReporterId == _currentUser.UserId,
                    "ThreatCaseId", false).ToArray();

            return View(threatCases);
        }

    }
}
