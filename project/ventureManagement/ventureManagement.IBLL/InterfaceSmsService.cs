using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceSmsService : InterfaceBaseService<Sms>
    {
        Sms Find(Expression<Func<Sms, bool>> whereLamdba);
        IQueryable<Sms> FindList(Expression<Func<Sms, bool>> whereLamdba);
    }
}
