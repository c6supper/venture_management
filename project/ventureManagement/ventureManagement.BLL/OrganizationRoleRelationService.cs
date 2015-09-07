using System;
using System.Diagnostics;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.IDAL;

namespace VentureManagement.BLL
{
    public class OrganizationRoleRelationService : BaseService<OrganizationRoleRelation>,
        InterfaceOrganizationRoleRelationService
    {
        private readonly OrganizationService _organizationService = new OrganizationService();
        private readonly RoleService _roleService = new RoleService();

        public OrganizationRoleRelationService()
            : base(RepositoryFactory.OrganizationRoleRelationRepository)
        {
        }

        public override bool Initilization()
        {
            try
            {
                if (Find(User.USER_ADMIN, Role.ROLE_ADMIN) != null) return true;

                var organization = _organizationService.Find(Organization.ORGANIZATION_STSTEM);
                var role = _roleService.Find(Role.ROLE_ADMIN);

                var organizationRoleRelation = new OrganizationRoleRelation
                {
                    OrganizationId = organization.OrganizationId,
                    RoleId = role.RoleId,
                    Organization = organization,
                    Role = role,
                    Description = "系统管理组角色"
                };
                Add(organizationRoleRelation);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public OrganizationRoleRelation Find(string role, string organization)
        {
            try
            {
                return CurrentRepository.Find(u => u.Role.RoleName == role && u.Organization.OrganizationName == organization);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }
            return null;
        }
    }
}