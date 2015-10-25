using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using VentureManagement.Models;
using VentureManagement.Web.Areas.Report.Models;

namespace VentureManagement.Web.Areas.Report.Controllers
{
    public class ProjectThreatCaseReportController : ThreatCaseReportBaseController
    {
        //
        // GET: /Report/ProjectThreatCaseReport/
        public ActionResult Index(DateTime? from, DateTime? to)
        {
            var reportFrom = @from == null ? new DateTime(1990, 1, 1) : Convert.ToDateTime(@from);

            var reportTo = to == null ? new DateTime(3000, 1, 1) : Convert.ToDateTime(to);

            var tcDi = new Dictionary<string, ProjectThreatCaseReport>();
            foreach (var threatcase in _threatCaseService.FindList(tc => (tc.ThreatCaseFoundTime >= reportFrom && tc.ThreatCaseFoundTime <= reportTo),
                "ThreatCaseLevel", false).ToArray())
            {
                if (!tcDi.ContainsKey(threatcase.Project.ProjectName))
                {
                    var threatCaseReport = new ProjectThreatCaseReport()
                    {
                        ProjectName = threatcase.Project.ProjectName,
                    };
                    tcDi[threatCaseReport.ProjectName] = threatCaseReport;
                    switch (threatcase.ThreatCaseLevel)
                    {
                        case ThreatCase.THREATCASE_LEVEL_ORDINARY:
                            threatCaseReport.ThreatCaseLevelGeneral++;
                            break;
                        case ThreatCase.THREATCASE_LEVEL_MINOR:
                            threatCaseReport.ThreatCaseLevelLarger++;
                            break;
                        case ThreatCase.THREATCASE_LEVEL_MAJOR:
                            threatCaseReport.ThreatCaseLevelMajor++;
                            break;
                    }
                }
            }

            return this.View(tcDi.Values.ToArray());
        }

        public ActionResult Search(DateTime ?from, DateTime ?to)
        {
            var reportFrom = @from == null ? new DateTime(1990,1,1) : Convert.ToDateTime(@from);

            var reportTo = to == null ? new DateTime(3000, 1, 1) : Convert.ToDateTime(to);

            return RedirectToAction("Index", new { from = reportFrom,to = reportTo});
        }
    }
}
