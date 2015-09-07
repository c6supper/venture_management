namespace VentureManagement.BLL
{
    public class DatabaseInitilization
    {
        public static void Initilization()
        {
            var userService = new UserService();
            userService.Initilization();

            var roleService = new RoleService();
            roleService.Initilization();

            var organization = new OrganizationService();
            organization.Initilization();

            var userRoleRelation = new UserRoleRelationService();
            userRoleRelation.Initilization();

            var organizationRelation = new OrganizationRelationService();
            organizationRelation.Initilization();

            var organizationRoleRelation = new OrganizationRoleRelationService();
            organizationRoleRelation.Initilization();
        }
    }
}