using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceOrganizationRelationService : InterfaceBaseService<OrganizationRelation>
    {
        IQueryable<OrganizationRelation> FindList(Expression<Func<OrganizationRelation, bool>> whereLamdba, string orderName, bool isAsc);
        bool Exist(string superiorDepartment, string subordinateDepartment);
        List<int> GetChildrenOrgList(int orgId);
        List<string> GetParentOrgList(string org);
    }
}
