using System;
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
        public OrganizationService()
            : base(RepositoryFactory.OrganizationRepository)
        {
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

#if DEBUG
        public Organization Have(string organization)
        {
            return CurrentRepository.Find(u => u.OrganizationName == organization);
        }
#endif

        public bool Exist(string organization,int superiorDepartmentId)
        {
            return
                (CurrentRepository.FindList(u => u.OrganizationName == organization, String.Empty, false)
                    .ToArray()
                    .SelectMany(org => org.OrganizationRelation)
                    .Any(orgr => orgr.SuperiorDepartmentId == superiorDepartmentId));
        }

        public Organization Find(string organization, int superiorDepartmentId)
        {
            return CurrentRepository.FindList(u => u.OrganizationName == organization, String.Empty, false)
                .ToArray()
                .FirstOrDefault(org => org.OrganizationRelation
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