﻿using VentureManagement.DAL;
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
        public static InterfaceUserRepository UserRepository { get { return new UserRepository(); } }

        public static InterfaceRoleRepository RoleRepository { get { return new RoleRepository(); } }

        public static InterfaceUserRoleRelatioRepository UserRoleRelationRepository { get { return new UserRoleRelationRepository(); } }
    }
}
