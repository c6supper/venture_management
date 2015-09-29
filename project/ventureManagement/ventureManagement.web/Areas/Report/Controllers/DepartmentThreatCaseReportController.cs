﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using VentureManagement.Web.Areas.Report.Models;

namespace VentureManagement.Web.Areas.Report.Controllers
{
    public class DepartmentThreatCaseReportController : ThreatCaseReportBaseController
    {
        //
        // GET: /Report/DepartmentThreatCaseReport/
        public ActionResult Index(DateTime? from, DateTime? to)
        {
            var reportFrom = @from == null ? new DateTime(1990, 1, 1) : Convert.ToDateTime(@from);

            var reportTo = to == null ? new DateTime(3000, 1, 1) : Convert.ToDateTime(to);

            var tcDi = new Dictionary<string, ThreatCaseReport>();
            foreach (var threatcase in _threatCaseService.FindList(tc => (tc.ThreatCaseFoundTime >= reportFrom && tc.ThreatCaseFoundTime <= reportTo),
                "ThreatCaseLevel", false).ToArray())
            {
                if (!tcDi.ContainsKey(threatcase.Project.Organization.OrganizationName))
                {
                    var threatCaseReport = new ThreatCaseReport()
                    {
                        DepartmentName = threatcase.Project.Organization.OrganizationName
                    };
                    tcDi[threatCaseReport.DepartmentName] = threatCaseReport;
                    switch (threatcase.ThreatCaseLevel)
                    {
                        case "一般事故隐患":
                            threatCaseReport.ThreatCaseLevelGeneral++;
                            break;
                        case "较大事故隐患":
                            threatCaseReport.ThreatCaseLevelLarger++;
                            break;
                        case "重大事故隐患":
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
