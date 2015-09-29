using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VentureManagement.Web.Areas.Report.Controllers
{
    public class ProjectThreatCaseReportController : ThreatCaseReportBaseController
    {
        //
        // GET: /Report/ThreatCaseReport/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string starttime, string endtime)
        {
            return View();
        }
    }
}
