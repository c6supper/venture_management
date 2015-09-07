using System;
using System.Diagnostics;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.BLL;
using VentureManagement.DAL;

namespace VentureManagement.BLL
{
    public class RoleService : BaseService<Role>, InterfaceRoleService
    {
        public RoleService() : base(RepositoryFactory.RoleRepository)
        {
        }

        public override bool Initilization()
        {
            try
            {
                if (Find(Role.ROLE_ADMIN) != null) return true;

                var adminRole = new Role
                {
                    RoleName = Role.ROLE_ADMIN,
                    Description = "系统管理员"
                };
                Add(adminRole);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public Role Find(string role) { return CurrentRepository.Find(u => u.RoleName == role); }
    }
}
