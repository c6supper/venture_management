using ventureManagement.IDAL;
using ventureManagement.Models;
using VentureManagement.DAL;

namespace ventureManagement.DAL
{
    /// <summary>
    /// 用户仓库
    /// <remarks>创建：2014.02.03</remarks>
    /// </summary>
    class UserRepository : BaseRepository<User>, InterfaceUserRepository
    {
    }
}
