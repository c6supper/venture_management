using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Controllers;

namespace VentureManagement.Web.Areas.Report.Controllers
{
    public class ThreatCaseReportBaseController : BaseController
    {
        protected readonly InterfaceThreatCaseService _threatCaseService;
        protected readonly InterfaceProjectRelationService _projectRelationService = new ProjectRelationService();
        protected readonly InterfaceOrganizationRelationService _orgrService = new OrganizationRelationService();
        
        protected const string _template = "~/Areas/Threat/Content/CorrectionTemplate.xml";
        protected readonly ThreatCorrection _correctionTemplate = ThreatCorrection.Deserialize(System.Web.HttpContext.Current.Server.MapPath(_template));

        protected ThreatCaseReportBaseController()
        {
            _threatCaseService = new ThreatCaseService(_currentOrgList);
        }
    }
}
