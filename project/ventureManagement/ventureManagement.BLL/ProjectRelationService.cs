using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class ProjectRelationService : BaseService<ProjectRelation>, InterfaceProjectRelationService
    {
        public ProjectRelationService()
            : base(RepositoryFactory.ProjectRelationRepository)
        {
        }
    }
}