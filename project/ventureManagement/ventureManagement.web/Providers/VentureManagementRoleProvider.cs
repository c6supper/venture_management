using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.Security;
using VentureManagement.BLL;
using VentureManagement.Models;

namespace VentureManagement.Web.Providers
{
    public class VentureManagementRoleProvider : RoleProvider
    {
        private readonly UserRoleRelationService _userRoleRelationService = new UserRoleRelationService();
        private readonly UserOrganizationRelationService _userOrganizationRelationService = new UserOrganizationRelationService();
        private readonly OrganizationRoleRelationService _orgRoleRelationService = new OrganizationRoleRelationService();

        public override bool IsUserInRole(string username, string roleName)
        {
            try
            {
                if (_userRoleRelationService.Exist(username, roleName) || _userRoleRelationService.IsAdmin(username))
                    return true;

                foreach (var userOrganizationRelation in _userOrganizationRelationService.FindList(username))
                {
                    if (_orgRoleRelationService.Exist(userOrganizationRelation.Organization.OrganizationName, roleName))
                        return true;
                }

                return false;
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
                foreach (var userRoleRelation in _userRoleRelationService.FindListByUser(username).ToArray())
                {
                    foreach (var permission in Role.RoleValueToPermissions(userRoleRelation.Role.RoleValue).Where(permission => !perimissionList.Contains(permission)))
                    {
                        perimissionList.Add(permission);
                    }
                }

                //foreach (var userOrganizationRelation in _userOrganizationRelationService.FindList(username).ToArray())
                //{
                //    foreach (var orgRoleRelation in _orgRoleRelationService.FindList(userOrganizationRelation.Organization.OrganizationName).ToArray())
                //    {
                //        if (!roles.Contains(orgRoleRelation.Role.RoleName))
                //            roles.Add(orgRoleRelation.Role.RoleName);
                //    }
                //}
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