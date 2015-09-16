using System;
using System.Diagnostics;
using System.Linq;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class ProjectRelationService : BaseService<ProjectRelation>, InterfaceProjectRelationService
    {
        public ProjectRelationService()
            : base(RepositoryFactory.ProjectRelationRepository)
        {
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