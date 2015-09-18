using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class ProjectService : BaseService<VMProject>, InterfaceProjectService
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

        public VMProject Find(string project, int superProjectId)
        {
            return CurrentRepository.FindList(u => u.ProjectName == project, string.Empty, false)
                .ToArray()
                .FirstOrDefault(prj => prj.ProjectRelation
                    .Any(prjr => prjr.SuperProjectId == superProjectId));
        }

        public VMProject Find(int projectId)
        {
            return CurrentRepository.Find(org => org.ProjectId == projectId);
        }

        public IQueryable<VMProject> FindList(Expression<Func<VMProject, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }

        public override bool Initilization()
        {
            try
            {
                if (Find(1) != null) return true;

                var project = new VMProject
                {
                    ProjectName = VMProject.PROJECT_ROOT,
                    ProjectLocation = "",
                    OrganizationId = 1
                };
                Add(project);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }
    }
}