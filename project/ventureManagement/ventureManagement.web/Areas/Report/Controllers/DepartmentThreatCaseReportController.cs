using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentureManagement.Web.Areas.Report.Models;

namespace VentureManagement.Web.Areas.Report.Controllers
{
    public class DepartmentThreatCaseReportController : ThreatCaseReportBaseController
    {
        //
        // GET: /Report/DepartmentThreatCaseReport/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string starttime, string endtime)
        {
            DateTime startTime = Convert.ToDateTime(starttime.Replace("\"", ""));
            DateTime endTime = Convert.ToDateTime(endtime.Replace("\"", ""));

            var list = new List<ThreatCaseReport>();
            foreach (var threatcase in _threatCaseService.FindList(tc => (tc.ThreatCaseFoundTime > startTime || tc.ThreatCaseFoundTime < endTime), 
                "ThreatCaseLevel", false).ToArray())
            {
                var i = 0;
                //foreach (var item in list)
                for (i = 0; i < list.Count; i++)
                {
                    if (list[i].DepartmentName == threatcase.Project.Organization.OrganizationName)
                    {
                        if (threatcase.ThreatCaseLevel == "一般事故隐患")
                        {
                            list[i].ThreatCaseLevelGeneral++;
                        }
                        else if (threatcase.ThreatCaseLevel == "较大事故隐患")
                        {
                            list[i].ThreatCaseLevelLarger++;
                        }
                        else if (threatcase.ThreatCaseLevel == "重大事故隐患")
                        {
                            list[i].ThreatCaseLevelMajor++;
                        }
                        break;
                    }
                }

                if (i > list.Count)
                {
                    ThreatCaseReport tcr = new ThreatCaseReport();
                    tcr.DepartmentName = threatcase.Project.Organization.OrganizationName;
                    list.Add(tcr);
                }
            }

            return View(list);
        }
    }
}
