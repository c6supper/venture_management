using System;
using System.Diagnostics;
using System.Linq;
using VentureManagement.IBLL;
using VentureManagement.DAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class OrganizationRelationService : BaseService<OrganizationRelation>, InterfaceOrganizationRelationService
    {
        readonly OrganizationService _organizationService = new OrganizationService();

        public OrganizationRelationService()
            : base(RepositoryFactory.OrganizationRelationRepository)
        {
        }

        public IQueryable<OrganizationRelation> FindList(string organization)
        {
            return CurrentRepository.FindList(u => u.SuperiorDepartment.OrganizationName == organization, string.Empty, false);
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
                    SuperiorDepartmentId = _organizationService.Find(Organization.ORGANIZATION_STSTEM).OrganizationId,
                    SubordinateDepartmentId = _organizationService.Find("集团公司安全部").OrganizationId,
                    SuperiorDepartment = _organizationService.Find(Organization.ORGANIZATION_STSTEM),
                    SubordinateDepartment = _organizationService.Find("集团公司安全部"),
                    Description = "debug"
                });

                Add(new OrganizationRelation
                {
                    SuperiorDepartmentId = _organizationService.Find("集团公司安全部").OrganizationId,
                    SubordinateDepartmentId = _organizationService.Find("第一分局").OrganizationId,
                    SuperiorDepartment = _organizationService.Find("集团公司安全部"),
                    SubordinateDepartment = _organizationService.Find("第一分局"),
                    Description = "debug"
                });

                Add(new OrganizationRelation
                {
                    SuperiorDepartmentId = _organizationService.Find("第一分局").OrganizationId,
                    SubordinateDepartmentId = _organizationService.Find("第一工程项目部").OrganizationId,
                    SuperiorDepartment = _organizationService.Find("第一分局"),
                    SubordinateDepartment = _organizationService.Find("第一工程项目部"),
                    Description = "debug"
                });

                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
            }
#endif
        }

    }
}