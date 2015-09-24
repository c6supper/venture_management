using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
                    Description = "系统管理员",
                    RoleValue = int.MaxValue
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

        public Role Find(string roleName) { return CurrentRepository.Find(u => u.RoleName == roleName); }

        public IQueryable<Role> FindPageList(int pageIndex, int pageSize, out int totalRecord, string orderName)
        {
            return CurrentRepository.FindPageList(pageIndex, pageSize, out totalRecord, u => true, orderName, false);
        }

        public IQueryable<Role> FindPageList(int pageIndex, int pageSize, out int totalRecord,
            Expression<Func<Role, bool>> whereLamdba)
        {
            return CurrentRepository.FindPageList(pageIndex, pageSize, out totalRecord, whereLamdba, "RoleName", false);
        }

        public IQueryable<Role> FindList(Expression<Func<Role, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }

        public Role Find(int roleId) { return CurrentRepository.Find(u => u.RoleId == roleId); }
    }
}
