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
        IQueryable<Organization> FindList(Expression<Func<Organization, bool>> whereLamdba, string orderName, bool isAsc);
    }
}
