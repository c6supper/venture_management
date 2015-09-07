using System;
using System.Diagnostics;
using System.Linq;
using VentureManagement.Models;
using VentureManagement.DAL;
using VentureManagement.IBLL;

namespace VentureManagement.BLL
{
    public class UserRoleRelationService : BaseService<UserRoleRelation>, InterfaceRoleRelationService
    {
        private readonly UserService _userService = new UserService();
        private readonly RoleService _roleService = new RoleService();

        public UserRoleRelationService()
            : base(RepositoryFactory.UserRoleRelationRepository)
        {
        }

        public override bool Initilization()
        {
            try
            {
                if (Find(User.USER_ADMIN, Role.ROLE_ADMIN) != null) return true;

                var user = _userService.Find(User.USER_ADMIN);
                var role = _roleService.Find(Role.ROLE_ADMIN);

                var adminUserRoleRelation = new UserRoleRelation
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId,
                    User = user,
                    Role = role
                };
                Add(adminUserRoleRelation);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public UserRoleRelation Find(string user, string role)
        {
            try
            {
                return CurrentRepository.Find(u => u.User.UserName == user && u.Role.RoleName == role);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }
            return null;
        }

        public bool Exist(string username, string role)
        {
            try
            {
                return CurrentRepository.Exist(u => u.User.UserName == username && u.Role.RoleName == role);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public IQueryable<UserRoleRelation> FindList(string user)
        {
            try
            {
                return CurrentRepository.FindList(u => u.User.UserName == user, string.Empty, false);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }
            return null;
        }
    }
}
