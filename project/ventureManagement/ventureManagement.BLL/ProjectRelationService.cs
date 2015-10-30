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
        private readonly HashSet<int> _orgHash;
        public ProjectRelationService(HashSet<int> orgHash)
            : base(RepositoryFactory.ProjectRelationRepository)
        {
            _orgHash = orgHash;
        }

        public ProjectRelationService()
            : base(RepositoryFactory.ProjectRelationRepository)
        {
        }

        public IQueryable<ProjectRelation> FindList(Expression<Func<ProjectRelation, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
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
    }
}