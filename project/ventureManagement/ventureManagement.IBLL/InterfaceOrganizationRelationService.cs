using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceOrganizationRelationService : InterfaceBaseService<OrganizationRelation>
    {
        IQueryable<OrganizationRelation> FindList(string organization);
        bool Exist(string superiorDepartment, string subordinateDepartment);
        List<int> GetChildrenOrgList(string org);
    }
}
