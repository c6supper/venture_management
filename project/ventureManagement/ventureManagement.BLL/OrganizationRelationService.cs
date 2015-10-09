using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.IBLL;
using VentureManagement.DAL;
using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class OrganizationRelationService : BaseService<OrganizationRelation>, InterfaceOrganizationRelationService
    {
        readonly OrganizationService _organizationService = new OrganizationService();

        private readonly List<int> _currentOrgList;
        public OrganizationRelationService(List<int> currentOrgList)
            : base(RepositoryFactory.OrganizationRelationRepository)
        {
            _currentOrgList = currentOrgList;
            //if (currentOrgList != null)
            //{
            //    CurrentRepository.EntityFilterEvent += OrganizationRelationFilterEvent;                
            //}
        }

        public OrganizationRelationService()
            : base(RepositoryFactory.OrganizationRelationRepository)
        {
        }

        //private object OrganizationRelationFilterEvent(object sender, FileterEventArgs e)
        //{
        //    var orgrs = e.EventArg as IQueryable<OrganizationRelation>;
        //    Debug.Assert(orgrs != null, "orgrs != null");

        //    var filteredOrganizationRelations = new List<OrganizationRelation>();
        //    foreach (var orgId in _currentOrgList)
        //    {
        //        filteredOrganizationRelations.AddRange(orgrs.Where(orgr => orgr.SuperiorDepartmentId == orgId ||
        //            orgr.SubordinateDepartmentId == orgId));
        //    }

        //    return filteredOrganizationRelations.AsQueryable();
        //}

        public IQueryable<OrganizationRelation> FindList(Expression<Func<OrganizationRelation, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, string.Empty, false);
        }

        public bool Exist(string superiorDepartment, string subordinateDepartment)
        {
            try
            {
                return CurrentRepository.Exist(u => u.SubordinateDepartment.OrganizationName == subordinateDepartment 
                    && u.SuperiorDepartment.OrganizationName == superiorDepartment);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
        }

        public override bool Initilization()
        {
#if DEBUG
            if (Exist(Organization.ORGANIZATION_STSTEM, "集团公司安全部")) return true;
            try
            {
                Add(new OrganizationRelation
                {
                    SuperiorDepartmentId = _organizationService.Have(Organization.ORGANIZATION_STSTEM).OrganizationId,
                    SubordinateDepartmentId = _organizationService.Have("集团公司安全部").OrganizationId,
                    SuperiorDepartment = _organizationService.Have(Organization.ORGANIZATION_STSTEM),
                    SubordinateDepartment = _organizationService.Have("集团公司安全部"),
                    Description = "debug"
                });

                Add(new OrganizationRelation
                {
                    SuperiorDepartmentId = _organizationService.Have("集团公司安全部").OrganizationId,
                    SubordinateDepartmentId = _organizationService.Have("第一分局").OrganizationId,
                    SuperiorDepartment = _organizationService.Have("集团公司安全部"),
                    SubordinateDepartment = _organizationService.Have("第一分局"),
                    Description = "debug"
                });

                Add(new OrganizationRelation
                {
                    SuperiorDepartmentId = _organizationService.Have("第一分局").OrganizationId,
                    SubordinateDepartmentId = _organizationService.Have("第一工程项目部").OrganizationId,
                    SuperiorDepartment = _organizationService.Have("第一分局"),
                    SubordinateDepartment = _organizationService.Have("第一工程项目部"),
                    Description = "debug"
                });
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
#endif
            return true;
        }

        public List<int> GetChildrenOrgList(string org)
        {
            var childrenList = new List<int>();

            foreach (var orgr in CurrentRepository.FindList(orgr => orgr.SuperiorDepartment.OrganizationName == org,
                "OrganizationRelationId", false).ToArray())
            {
                if (orgr.SubordinateDepartment != null)
                    childrenList.AddRange(GetChildrenOrgList(orgr.SubordinateDepartment.OrganizationName));

                childrenList.Add(orgr.SubordinateDepartmentId);
            }

            return childrenList;
        }
    }
}