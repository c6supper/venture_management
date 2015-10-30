using VentureManagement.IDAL;
using VentureManagement.Models;
using VentureManagement.DAL;

namespace VentureManagement.DAL
{
    /// <summary>
    /// 用户仓库
    /// <remarks>创建：2014.02.03</remarks>
    /// </summary>
    class UserRepository : BaseRepository<User>, InterfaceUserRepository
    {
        public UserRepository()
        {
            RegisterProxyIncludePath("UserRoleRelations.Role"); 
        }
    }
}
