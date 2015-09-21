using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class UserOrganizationRelationService : BaseService<UserOrganizationRelation>, InterfaceUserOrganizationRelationService
    {
        private readonly UserService _userService = new UserService();
        private readonly OrganizationService _organizationService = new OrganizationService();

        private readonly List<int> _currentOrgList;
        public UserOrganizationRelationService(List<int> currentOrgList)
            : base(RepositoryFactory.UserOrganizationRelationRepository)
        {
            _currentOrgList = currentOrgList;
            CurrentRepository.EntityFilterEvent += UserOrganizationRelationFilterEvent;
        }

        public UserOrganizationRelationService()
            : base(RepositoryFactory.UserOrganizationRelationRepository)
        {
        }

        private object UserOrganizationRelationFilterEvent(object sender, FileterEventArgs e)
        {
            var uors = e.EventArg as IQueryable<UserOrganizationRelation>;

            return _currentOrgList.Aggregate(uors, (current, orgId) =>
                current.Where(uor => uor.OrganizationId == orgId).Concat(current));
        }

        public override bool Initilization()
        {
            try
            {
                if (Find(User.USER_ADMIN,Organization.ORGANIZATION_STSTEM) != null) return true;

                var user = _userService.Find(User.USER_ADMIN);
                var organization = _organizationService.Have(Organization.ORGANIZATION_STSTEM);

                var adminUserOrganizationRelation = new UserOrganizationRelation
                {
                    UserId = user.UserId,
                    OrganizationId = organization.OrganizationId,
                    User = user,
                    Organization = organization
                };
                Add(adminUserOrganizationRelation);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public UserOrganizationRelation Find(string user, string organization)
        {
            try
            {
                return CurrentRepository.Find(u => u.User.UserName == user && u.Organization.OrganizationName == organization);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }
            return null;
        }

        public IQueryable<UserOrganizationRelation> FindList(string user)
        {
            try
            {
                return CurrentRepository.FindList(u => u.User.UserName == user,string.Empty,false);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }
            return null;
        }
    }
}