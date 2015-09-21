using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class ProjectService : BaseService<VMProject>, InterfaceProjectService
    {
        private readonly List<int> _currentOrgList;
        public ProjectService(List<int> currentOrgList)
            : base(RepositoryFactory.ProjectRepository)
        {
            _currentOrgList = currentOrgList;
            CurrentRepository.EntityFilterEvent += ProjectFilterEvent;
        }

        public ProjectService()
            : base(RepositoryFactory.ProjectRepository)
        {
        }

        private object ProjectFilterEvent(object sender, FileterEventArgs e)
        {
            var vmps = e.EventArg as IQueryable<VMProject>;

            return _currentOrgList.Aggregate(vmps, (current, orgId) =>
                current.Where(vmp => vmp.OrganizationId == orgId).Concat(current));
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