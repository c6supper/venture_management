using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Controllers;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    public class ThreatBaseController : BaseController
    {
        //
        // GET: /Threat/ThreatBase/
        // ReSharper disable once InconsistentNaming
        protected const string _template = "~/Areas/Threat/Content/CorrectionTemplate.xml";
        // ReSharper disable once InconsistentNaming
        protected readonly ThreatCorrection _correctionTemplate = ThreatCorrection.Deserialize(System.Web.HttpContext.Current.Server.MapPath(_template));
        // ReSharper disable once InconsistentNaming
        protected readonly InterfaceThreatCaseService _threatCaseService;

        protected ThreatBaseController()
        {
            _threatCaseService = new ThreatCaseService(_currentOrgList);
        }
    }
}
