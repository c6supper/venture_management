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
                if (Find(Role.ROLE_ADMIN) == null)
                {
                    var role = new Role
                    {
                        RoleName = Role.ROLE_ADMIN,
                        Description = "系统管理员",
                        RoleValue = long.MaxValue
                    };
                    Add(role);
                }
                
                if (Find(Role.ROLE_GROUP) == null)
                {
                    var role = new Role
                    {
                        RoleName = Role.ROLE_GROUP,
                        Description = "集团安全管理人员",
                        RoleValue = 0
                    };
                    Add(role);
                }

                if (Find(Role.ROLE_BRANCH) == null)
                {
                    var role = new Role
                    {
                        RoleName = Role.ROLE_BRANCH,
                        Description = "分局安全管理人员",
                        RoleValue = 0
                    };
                    Add(role);
                }

                if (Find(Role.ROLE_PROJECT_INSPECTOR) == null)
                {
                    var role = new Role
                    {
                        RoleName = Role.ROLE_PROJECT_INSPECTOR,
                        Description = "项目部安全巡检员",
                        RoleValue = 0
                    };
                    Add(role);
                }

                if (Find(Role.ROLE_PROJECT_LEADER) == null)
                {
                    var role = new Role
                    {
                        RoleName = Role.ROLE_PROJECT_LEADER,
                        Description = "项目负责人",
                        RoleValue = 0
                    };
                    Add(role);
                }
                
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
