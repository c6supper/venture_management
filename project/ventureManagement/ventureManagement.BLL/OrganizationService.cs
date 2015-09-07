using System;
using System.Diagnostics;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class OrganizationService : BaseService<Organization>, InterfaceOrganizationService
    {
        public OrganizationService()
            : base(RepositoryFactory.OrganizationRepository)
        {
        }

        public override bool Initilization()
        {
            try
            {
                if (Find(Organization.ORGANIZATION_STSTEM) != null) return true;

                var organization = new Organization
                {
                    OrganizationName = Organization.ORGANIZATION_STSTEM,
                    Description = "系统管理组"
                };
                Add(organization);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public Organization Find(string organization)
        {
            return CurrentRepository.Find(u => u.OrganizationName == organization);
        }
    }
}