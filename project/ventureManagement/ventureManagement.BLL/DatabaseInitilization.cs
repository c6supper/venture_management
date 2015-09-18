namespace VentureManagement.BLL
{
    public class DatabaseInitilization
    {
        public static void Initilization()
        {
            var organization = new OrganizationService();
            organization.Initilization();

            var userService = new UserService();
            userService.Initilization();

            var userOrganizationRelationService = new UserOrganizationRelationService();;
            userOrganizationRelationService.Initilization();

            var roleService = new RoleService();
            roleService.Initilization();


            var userRoleRelation = new UserRoleRelationService();
            userRoleRelation.Initilization();

            var organizationRelation = new OrganizationRelationService();
            organizationRelation.Initilization();

            var organizationRoleRelation = new OrganizationRoleRelationService();
            organizationRoleRelation.Initilization();

            var projectService = new ProjectService();
            projectService.Initilization();
        }
    }
}