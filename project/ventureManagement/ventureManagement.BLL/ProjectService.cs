using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class ProjectService : BaseService<Project>, InterfaceProjectService
    {
        public ProjectService()
            : base(RepositoryFactory.ProjectRepository)
        {
        }
    }
}