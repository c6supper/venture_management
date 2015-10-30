using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentureManagement.BLL;
using VentureManagement.IBLL;
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

            InterfaceOrganizationService orgService = new OrganizationService();
            var org = orgService.Find(threatCase.Project.OrganizationId);

            Organization parentOrg = null,grandpaOrg = null;

            try
            {
                // ReSharper disable once PossibleNullReferenceException
                parentOrg = orgService.Find(org.AsSubOrganizationRelations.FirstOrDefault().SuperiorDepartmentId);
                // ReSharper disable once PossibleNullReferenceException
                grandpaOrg = orgService.Find(parentOrg.AsSubOrganizationRelations.FirstOrDefault().SuperiorDepartmentId);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            ViewData["Note"] = ((grandpaOrg == null || grandpaOrg.OrganizationName == Organization.ORGANIZATION_STSTEM) ? "" : grandpaOrg.OrganizationName) +
                ((parentOrg == null || parentOrg.OrganizationName == Organization.ORGANIZATION_STSTEM) ? "" : parentOrg.OrganizationName) +
                org.OrganizationName + "在" +
                threatCase.Project.ProjectLocation + "场所存在\"" + 
                threatCase.ThreatCaseCategory + "\"" + " \"" +
                threatCase.ThreatCaseType + "\"类安全隐患，隐患类别属于:" + threatCase.ThreatCaseLevel + ",请施工方于" + 
                threatCase.ThreatCaseLimitTime.ToString("yyyy年MM月dd日")+"之前完成整改。";
            return View();
        }
    }
}
