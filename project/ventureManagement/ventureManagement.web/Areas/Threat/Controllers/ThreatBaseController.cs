using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Web.Controllers;

namespace VentureManagement.Web.Areas.Threat.Controllers
{
    public class ThreatBaseController : BaseController
    {
        //
        // GET: /Threat/ThreatBase/
        protected readonly InterfaceThreatCaseService _threatCaseService;

        protected ThreatBaseController()
        {
            _threatCaseService = new ThreatCaseService(_currentOrgList);
        }
    }
}
