using System;
using System.Data.Entity;
using System.Diagnostics;
using VentureManagement.Models;

namespace VentureManagement.DAL
{
    /// <summary>
    /// 数据上下文
    /// <remarks>创建：2014.02.03</remarks>
    /// 一些人则认为不应该静态化、单例化。理由：1、MSDN说明其是轻量的，
    /// 创建不需要很大开销；2、不是线程安全的对象；3、有数据容器的性质（跟踪）。
    /// </summary>
    public class VentureManagementDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleRelation> UserRoleRelations { get; set; }
        public DbSet<UserConfig> UserConfig { get; set; }
        public DbSet<OrganizationRoleRelation> UserOrganizationRoleRelation { get; set; }
        public DbSet<OrganizationRelation> UserOrganizationRelation { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public VentureManagementDbContext()
            : base("DefaultConnection")
        {
            base.Configuration.LazyLoadingEnabled = true;
            try
            {
                Database.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }
        }
    }
}
