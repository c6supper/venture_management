using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Web.Controllers;

namespace VentureManagement.Web.Areas.Project.Controllers
{
    public class ProjectBaseController : BaseController
    {
        //
        // GET: /Project/ProjectBase/
        protected readonly InterfaceProjectService _projectService;
        protected readonly InterfaceProjectRelationService _projectRelationService;

        protected ProjectBaseController()
        {
            _projectService = new ProjectService(_currentOrgList);
            _projectRelationService = new ProjectRelationService(_currentOrgList);
        }
    }
}
