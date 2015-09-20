using System.Linq;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceUserOrganizationRelationService : InterfaceBaseService<UserOrganizationRelation>
    {
        IQueryable<UserOrganizationRelation> FindList(string user);
    }
}