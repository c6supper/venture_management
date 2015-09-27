using System;
using System.Linq;
using System.Linq.Expressions;
using VentureManagement.Models;
using VentureManagement.IBLL;

namespace VentureManagement.IBLL
{
    /// <summary>
    /// 用户相关接口
    /// <remarks>
    /// 创建：2014.02.09
    /// 修改：2014.02.17
    /// </remarks>
    /// </summary>
    public interface InterfaceUserService : InterfaceBaseService<User>
    {
        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>布尔值</returns>
        bool Exist(string userName);

        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        User Find(int userId);

        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        User Find(string userName);

        int Count();

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="order">排序：0-ID升序（默认），1ID降序，2注册时间升序，3注册时间降序，4登录时间升序，5登录时间降序</param>
        /// <returns></returns>
        IQueryable<User> FindPageList(int pageIndex, int pageSize, out int totalRecord, int order);

        IQueryable<User> FindList(Expression<Func<User, bool>> whereLamdba, string orderName, bool isAsc);
    }
}
