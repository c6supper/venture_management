using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class ProjectRelationService : BaseService<ProjectRelation>, InterfaceProjectRelationService
    {
        private readonly List<int> _currentOrgList;
        public ProjectRelationService(List<int> currentOrgList)
            : base(RepositoryFactory.ProjectRelationRepository)
        {
            _currentOrgList = currentOrgList;
            CurrentRepository.EntityFilterEvent += ProjectRelationFilterEvent;
        }

        public ProjectRelationService()
            : base(RepositoryFactory.ProjectRelationRepository)
        {
        }
        
        private object ProjectRelationFilterEvent(object sender, FileterEventArgs e)
        {
            var prs = e.EventArg as IQueryable<ProjectRelation>;

            return _currentOrgList.Aggregate(prs, (current, orgId) =>
               current.Where(pr => pr.SuperProject.OrganizationId == orgId).Concat(current).Distinct());
        }

        public IQueryable<ProjectRelation> FindList(string projectName)
        {
            return CurrentRepository.FindList(prjr => prjr.SuperProject.ProjectName == projectName,
                "ProjectRelationId", false);
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
    }
}