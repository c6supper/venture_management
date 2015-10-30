namespace VentureManagement.BLL
{
    public class DatabaseInitilization
    {
        public static void Initilization()
        {
            var organization = new OrganizationService();
            organization.Initilization();

            var roleService = new RoleService();
            roleService.Initilization();

            var userService = new UserService();
            userService.Initilization();

            var organizationRelation = new OrganizationRelationService();
            organizationRelation.Initilization();

            var projectService = new ProjectService();
            projectService.Initilization();

            var projectRelationService = new ProjectRelationService();
            projectRelationService.Initilization();
        }
    }
}