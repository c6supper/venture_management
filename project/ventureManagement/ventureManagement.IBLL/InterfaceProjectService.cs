using System;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceProjectService : InterfaceBaseService<VMProject>
    {
        bool Exist(string project, int superProjectId);
        VMProject Find(string project, int superProjectId);
        VMProject Find(int projectId);
        IQueryable<VMProject> FindList(Expression<Func<VMProject, bool>> whereLamdba, string orderName, bool isAsc);

        IQueryable<VMProject> FindPageList(int pageIndex, int pageSize, out int totalRecord,
            Expression<Func<VMProject, bool>> whereLamdba);

        bool Delete(int projectId);
    }
}