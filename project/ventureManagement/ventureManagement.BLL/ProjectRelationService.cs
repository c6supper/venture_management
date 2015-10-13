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
    public class ProjectRelationService : BaseService<ProjectRelation>, InterfaceProjectRelationService
    {
        private readonly InterfaceProjectService _projectService = new ProjectService();
        private readonly List<int> _currentOrgList;
        public ProjectRelationService(List<int> currentOrgList)
            : base(RepositoryFactory.ProjectRelationRepository)
        {
            _currentOrgList = currentOrgList;
            //CurrentRepository.EntityFilterEvent += ProjectRelationFilterEvent;
        }

        public ProjectRelationService()
            : base(RepositoryFactory.ProjectRelationRepository)
        {
        }
        
        //private object ProjectRelationFilterEvent(object sender, FileterEventArgs e)
        //{
        //    var prs = e.EventArg as IQueryable<ProjectRelation>;
        //    Debug.Assert(prs != null, "prs != null");

        //    var filteredProjectRelation = new List<ProjectRelation>();
        //    foreach (var orgId in _currentOrgList)
        //    {
        //        filteredProjectRelation.AddRange(prs.Where(pr => pr.SuperProject.OrganizationId == orgId || 
        //            pr.SubProject.OrganizationId == orgId));
        //    }

        //    return filteredProjectRelation.AsQueryable();
        //}

        public IQueryable<ProjectRelation> FindList(Expression<Func<ProjectRelation, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }

        public bool Exist(string superiorProject, string subordinateProject)
        {
            try
            {
                return CurrentRepository.Exist(u => u.SubProject.ProjectName == subordinateProject
                    && u.SuperProject.ProjectName == superiorProject);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public override bool Initilization()
        {
#if DEBUG
            try
            {
                var projectService = new ProjectService();
                if (FindList(pr => pr.SubProject.ProjectName == "测试工程", "ProjectRelationId", false).Any()) return true;

                var projectRelation = new ProjectRelation
                {
                    SuperProjectId = VMProject.INVALID_PROJECT,
                    SubProjectId = projectService.FindList(t=>t.ProjectName == "测试工程","ProjectId",false).First().ProjectId
                };
                Add(projectRelation);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
#endif
            return true;
        }

        public List<string> GetParentProjectList(int projectId)
        {
            var parentList = new List<string>();

            foreach (var pror in FindList(pror => pror.SubProject.ProjectId == projectId,"ProjectRelationId", false).ToArray())
            {
                if (pror.SuperProjectId !=  VMProject.INVALID_PROJECT)
                    parentList.AddRange(GetParentProjectList(pror.SuperProject.ProjectId));

                parentList.Add(_projectService.Find(pror.SuperProjectId).ProjectName);
            }

            return parentList;
        }
    }
}