using System;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceUserOrganizationRelationService : InterfaceBaseService<UserOrganizationRelation>
    {
        IQueryable<UserOrganizationRelation> FindList(string user);

        IQueryable<UserOrganizationRelation> FindList(Expression<Func<UserOrganizationRelation, bool>> whereLamdba,
            string orderName, bool isAsc);
    }
}