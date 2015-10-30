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
        private readonly HashSet<int> _orgHash;
        public OrganizationService(HashSet<int> orgHash)
            : base(RepositoryFactory.OrganizationRepository)
        {
            _orgHash = orgHash;
            if (_orgHash != null)
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

            return orgs != null ? orgs.Where(o => _orgHash.Contains(o.OrganizationId)) : null;
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
                (CurrentRepository.Exist(u => u.OrganizationName == organization &&
                    u.AsSubOrganizationRelations.Any(orgr => orgr.SuperiorDepartmentId == superiorDepartmentId)));
        }
        
        public Organization Find(string organization, int superiorDepartmentId)
        {
            return CurrentRepository.Find(u => u.OrganizationName == organization && 
                u.AsSubOrganizationRelations.Any(orgr => orgr.SuperiorDepartmentId == superiorDepartmentId));
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

        public bool Delete(int orgId)
        {
            using (var transaction = CurrentRepository.BeginTransaction())
            {
                try
                {
                    var org = Find(orgId);
                    if (org == null)
                        return true;

                    var orgrService = new OrganizationRelationService();
                    if (orgrService.FindList(orgr => orgr.SubordinateDepartmentId == orgId,
                        "OrganizationId", false).ToArray().Any(orgr => !orgrService.Delete(orgr)))
                    {
                        transaction.Rollback();
                        return false;
                    }

                    var userService = new UserService();
                    if (userService.FindList(u => u.OrganizationId == orgId,
                        "UserId", false).Any())
                    {
                        transaction.Rollback();
                        return false;
                    }

                    if (!Delete(org))
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