using System;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.Models;

namespace VentureManagement.IBLL
{
    public interface InterfaceProjectService : InterfaceBaseService<Project>
    {
        bool Exist(string project, int? superProjectId);
        Project Find(string project, int superProjectId);
        Project Find(int projectId);
        IQueryable<Project> FindList(Expression<Func<Project, bool>> whereLamdba, string orderName, bool isAsc);
    }
}