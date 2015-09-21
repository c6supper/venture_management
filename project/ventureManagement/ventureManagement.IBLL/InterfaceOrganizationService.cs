using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceOrganizationService : InterfaceBaseService<Organization>
    {
        bool Exist(string organization, int superiorDepartmentId);
        Organization Find(string organization, int superiorDepartmentId);
        Organization Find(int organizationId);
        IQueryable<Organization> FindList(Expression<Func<Organization, bool>> whereLamdba, string orderName, bool isAsc);
        IQueryable<Organization> FindPageList(int pageIndex, int pageSize, out int totalRecord,
            Expression<Func<Organization, bool>> whereLamdba);
    }
}
