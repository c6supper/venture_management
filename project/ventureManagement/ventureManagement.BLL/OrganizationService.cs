using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class OrganizationService : BaseService<Organization>, InterfaceOrganizationService
    {
        private readonly List<int> _currentOrgList;
        public OrganizationService(List<int> currentOrgList)
            : base(RepositoryFactory.OrganizationRepository)
        {
            _currentOrgList = currentOrgList;
            if (currentOrgList != null)
            {
                CurrentRepository.EntityFilterEvent += OrganizationFilterEvent;                
            }
        }

        public OrganizationService()
            : base(RepositoryFactory.OrganizationRepository)
        {
        }

        private object OrganizationFilterEvent(object sender, FileterEventArgs e)
        {
            var orgs = e.EventArg as IQueryable<Organization>;
            Debug.Assert(orgs != null, "orgs != null");

            var filteredOrganization = new List<Organization>();
            foreach (var orgId in _currentOrgList)
            {
                filteredOrganization.AddRange(orgs.Where(org => org.OrganizationId == orgId));
            }

            return filteredOrganization.AsQueryable();
        }

        public override bool Initilization()
        {
            try
            {
                if (Have(Organization.ORGANIZATION_STSTEM) != null) return true;

                var organization = new Organization
                {
                    OrganizationName = Organization.ORGANIZATION_STSTEM,
                    Description = "系统管理组"
                };
                Add(organization);

#if DEBUG
                Add( new Organization
                {
                    OrganizationName = "集团公司安全部",
                    Description = "debug"
                });

                Add(new Organization
                {
                    OrganizationName = "第一分局",
                    Description = "debug"
                });

                Add(new Organization
                {
                    OrganizationName = "第一工程项目部",
                    Description = "debug"
                });
#endif
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public Organization Have(string organization)
        {
            return CurrentRepository.Find(u => u.OrganizationName == organization);
        }

        public bool Exist(string organization,int superiorDepartmentId)
        {
            return
                (CurrentRepository.FindList(u => u.OrganizationName == organization, String.Empty, false)
                    .ToArray()
                    .SelectMany(org => org.OrganizationRelations)
                    .Any(orgr => orgr.SuperiorDepartmentId == superiorDepartmentId));
        }
        
        public Organization Find(string organization, int superiorDepartmentId)
        {
            return CurrentRepository.FindList(u => u.OrganizationName == organization, String.Empty, false)
                .ToArray()
                .FirstOrDefault(org => org.OrganizationRelations
                    .Any(orgr => orgr.SuperiorDepartmentId == superiorDepartmentId));
        }

        public Organization Find(int organizationId)
        {
            return CurrentRepository.Find(org => org.OrganizationId == organizationId);
        }

        public IQueryable<Organization> FindList(Expression<Func<Organization, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }

        public IQueryable<Organization> FindPageList(int pageIndex, int pageSize, out int totalRecord,
            Expression<Func<Organization, bool>> whereLamdba)
        {
            return CurrentRepository.FindPageList(pageIndex, pageSize, out totalRecord, whereLamdba, "OrganizationName", false);
        }
    }
}