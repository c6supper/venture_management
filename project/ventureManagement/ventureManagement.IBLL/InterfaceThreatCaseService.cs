using System;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceThreatCaseService : InterfaceBaseService<ThreatCase>
    {
        bool Exist(int threatCaseId);
        ThreatCase Find(int threatCaseId);
        int Count();
        IQueryable<ThreatCase> FindList(Expression<Func<ThreatCase, bool>> whereLamdba, string orderName, bool isAsc);
        IQueryable<ThreatCase> FindPageList(int pageIndex, int pageSize, out int totalRecord,Expression<Func<ThreatCase, bool>> whereLamdba);
    }
}