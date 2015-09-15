using VentureManagement.DAL;
using VentureManagement.IDAL;

namespace VentureManagement.DAL
{
    /// <summary>
    /// 简单工厂？
    /// <remarks>创建：2014.02.03</remarks>
    /// </summary>
    public static class RepositoryFactory
    {
        /// <summary>
        /// 用户仓储
        /// </summary>

        public static InterfaceProjectRepository ProjectRepository { get { return new ProjectRepository(); } }
        public static InterfaceProjectRelationRepository ProjectRelationRepository { get { return new ProjectRelationRepository(); } }
        public static InterfaceUserProjectRelationRepository UserProjectRelationRepository { get { return new UserProjectRelationRepository(); } }
        
        public static InterfaceUserRepository UserRepository { get { return new UserRepository(); } }

        public static InterfaceRoleRepository RoleRepository { get { return new RoleRepository(); } }

        public static InterfaceUserRoleRelationRepository UserRoleRelationRepository { get { return new UserRoleRelationRepository(); } }

        public static InterfaceOrganizationRelationRepository OrganizationRelationRepository { get { return new OrganizationRelationRepository(); } }

        public static InterfaceOrganizationRoleRelationRepository OrganizationRoleRelationRepository { get { return new OrganizationRoleRelationRepository(); } }

        public static InterfaceOrganizationRepository OrganizationRepository { get { return new OrganizationRepository(); } }

        public static InterfaceUserOrganizationRelationRepository UserOrganizationRelationRepository { get { return new UserOrganizationRelationRepository(); } }
    }
}
