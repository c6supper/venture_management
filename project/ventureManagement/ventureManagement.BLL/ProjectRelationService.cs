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
    }
}