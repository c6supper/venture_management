using System.Runtime.Remoting.Messaging;
using VentureManagement.DAL;

namespace VentureManagement.DAL
{
    /// <summary>
    /// 上下文简单工厂
    /// <remarks>
    /// 创建：2014.02.05
    /// </remarks>
    /// </summary>
    public class ContextFactory
    {
        //关于DbContext的单例问题，看了一些文章讲DbContex是轻量级的，创建的开销不大，另一个DbContext并不能保证线程安全。
        //对于DbContext单例化、静态化很多人反对，但每个操作都进行创建和销毁也不合理，实现一个请求内单例还是比较合适。
        //看到@Kencery这么写，我特意查了些资料：MSDN中讲CallContext提供对每个逻辑执行线程都唯一的数据槽，而在WEB程序里，每一个请求就是一个逻辑线程
        //所以使用CallContext来实现单个请求之内的DbContext单例是合理的。

        /// <summary>
        /// 获取当前数据上下文
        /// </summary>
        /// <returns></returns>
        public static VentureManagementDbContext GetCurrentContext()
        {
            VentureManagementDbContext nContext = CallContext.GetData("ventureManagementContext") as VentureManagementDbContext;
            if (nContext == null)
            {
                nContext = new VentureManagementDbContext();
                CallContext.SetData("ventureManagementContext", nContext);
            }
            return nContext;
        }
    }
}
