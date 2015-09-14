using System;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.Models;
using VentureManagement.IBLL;

namespace VentureManagement.IBLL
{
    public interface InterfaceRoleService : InterfaceBaseService<Role>
    {
        IQueryable<Role> FindPageList(int pageIndex, int pageSize, out int totalRecord, string orderName);

        IQueryable<Role> FindList(Expression<Func<Role, bool>> whereLamdba, string orderName, bool isAsc);
    }
}
