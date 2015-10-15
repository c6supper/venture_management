using System;
using System.Data.Entity;
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

#if DEBUG
                user = _userService.Find("reporter");
                role = _roleService.Find(Role.ROLE_PROJECT_INSPECTOR);

                var userRoleRelation = new UserRoleRelation
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId,
                    User = user,
                    Role = role
                };
                Add(userRoleRelation);

                user = _userService.Find("projectOwner");
                role = _roleService.Find(Role.ROLE_PROJECT_LEADER);

                userRoleRelation = new UserRoleRelation
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId,
                    User = user,
                    Role = role
                };
                Add(userRoleRelation);

                user = _userService.Find("confirmer");
                role = _roleService.Find(Role.ROLE_BRANCH);

                userRoleRelation = new UserRoleRelation
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId,
                    User = user,
                    Role = role
                };
                Add(userRoleRelation);

                user = _userService.Find("reviewer");
                role = _roleService.Find(Role.ROLE_BRANCH);

                userRoleRelation = new UserRoleRelation
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId,
                    User = user,
                    Role = role
                };
                Add(userRoleRelation);
#endif
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public bool IsAdmin(string user)
        {
            return Exist(user, Role.ROLE_ADMIN);
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

        public IQueryable<UserRoleRelation> FindListByUser(string user)
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

        public IQueryable<UserRoleRelation> FindListByRole(int roleId)
        {
            try
            {
                return CurrentRepository.FindList(u => u.RoleId == roleId, string.Empty, false);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }
            return null;
        }

        public bool DeleteByUser(string userName)
        {
            using (var transaction = CurrentRepository.BeginTransaction())
            {
                try
                {
                    if (FindListByUser(userName).ToArray().Any(userRelation => !Delete(userRelation)))
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.Print(ex.Message);
                }
                transaction.Commit();
            }

            return true;
        }

        public bool DeleteByRole(int roleId)
        {
            using (var transaction = CurrentRepository.BeginTransaction())
            {
                try
                {
                    if (FindListByRole(roleId).ToArray().Any(userRelation => !Delete(userRelation)))
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.Print(ex.Message);
                }
                transaction.Commit();
            }

            return true;
        }
    }
}
