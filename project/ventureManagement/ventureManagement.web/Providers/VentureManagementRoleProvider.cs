using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.Security;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;

namespace VentureManagement.Web.Providers
{
    public class VentureManagementRoleProvider : RoleProvider
    {
        readonly InterfaceUserService _userService = new UserService();
        public override bool IsUserInRole(string username, string roleName)
        {
            try
            {
                var user = _userService.Find(username);
                return (user.Role.RoleName == roleName) || username.Equals(User.USER_ADMIN);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            var perimissionList = new List<string>();
            try
            {
                var user = _userService.Find(username);
                foreach (var permission in Role.RoleValueToPermissions(user.Role.RoleValue).Where(permission => !perimissionList.Contains(permission)))
                {
                    perimissionList.Add(permission);
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            return perimissionList.ToArray();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}