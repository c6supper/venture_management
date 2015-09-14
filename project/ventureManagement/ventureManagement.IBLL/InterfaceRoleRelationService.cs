using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceRoleRelationService : InterfaceBaseService<UserRoleRelation>
    {
        bool DeleteByUser(string userName);

        bool DeleteByRole(int roleId);
    }
}
