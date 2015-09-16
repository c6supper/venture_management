using System;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class ProjectService : BaseService<Project>, InterfaceProjectService
    {
        public ProjectService()
            : base(RepositoryFactory.ProjectRepository)
        {
        }

        public bool Exist(string project, int? superProjectId)
        {
            return
                (CurrentRepository.FindList(u => u.ProjectName == project, string.Empty, false)
                    .ToArray()
                    .SelectMany(prj => prj.ProjectRelation)
                    .Any(prjr => prjr.SuperProjectId == superProjectId));
        }

        public Project Find(string project, int superProjectId)
        {
            return CurrentRepository.FindList(u => u.ProjectName == project, string.Empty, false)
                .ToArray()
                .FirstOrDefault(prj => prj.ProjectRelation
                    .Any(prjr => prjr.SuperProjectId == superProjectId));
        }

        public Project Find(int projectId)
        {
            return CurrentRepository.Find(org => org.ProjectId == projectId);
        }

        public IQueryable<Project> FindList(Expression<Func<Project, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }
    }
}