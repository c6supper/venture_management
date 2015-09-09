using System;
using System.Diagnostics;
using System.Linq;
using VentureManagement.IBLL;
using VentureManagement.DAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class OrganizationRelationService : BaseService<OrganizationRelation>, InterfaceOrganizationRelationService
    {
        public OrganizationRelationService()
            : base(RepositoryFactory.OrganizationRelationRepository)
        {
        }

        public IQueryable<OrganizationRelation> FindList(string organization)
        {
            return CurrentRepository.FindList(u => u.SuperiorDepartment.OrganizationName == organization, string.Empty, false);
        }

        public override bool Initilization()
        {
            try
            {
#if DEBUG
#endif
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}