using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ventureManagement.Models;
using VentureManagement.IDAL;

namespace ventureManagement.IDAL
{
    /// <summary>
    /// 用户接口
    /// <remarks>创建：2014.02.03</remarks>
    /// </summary>
    public interface InterfaceUserRepository : InterfaceBaseRepository<User>
    {
    }
}
