using VentureManagement.Models;
using VentureManagement.DAL;
using VentureManagement.IBLL;

namespace VentureManagement.BLL
{
    public class UserRoleRelationService : BaseService<UserRoleRelation>, InterfaceRoleRelationService
    {
        public UserRoleRelationService()
            : base(RepositoryFactory.UserRoleRelationRepository)
        {
        }
    }
}
