using System;
using System.Diagnostics;
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
    }
}