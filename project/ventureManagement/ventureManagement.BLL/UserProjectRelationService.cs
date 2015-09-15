using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class UserProjectRelationService: BaseService<UserProjectRelation>, InterfaceUserProjectRelationService
    {
        public UserProjectRelationService()
            : base(RepositoryFactory.UserProjectRelationRepository)
        {
        }
    }
}