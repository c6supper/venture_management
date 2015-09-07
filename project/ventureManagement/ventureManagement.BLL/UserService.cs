using System;
using System.Diagnostics;
using System.Linq;
using Common;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.BLL;
using VentureManagement.DAL;

namespace VentureManagement.BLL
{
    /// <summary>
    /// 用户服务类
    /// <remarks>
    /// 创建：2014.02.12
    /// </remarks>
    /// </summary>
    public class UserService : BaseService<User>, InterfaceUserService
    {
        public UserService() : base(RepositoryFactory.UserRepository)
        {
            //default 
            try
            {
                if(Find("master") != null) return;

                var user = new User
                {
                    UserName = "master",
                    Password = Utility.DesEncrypt("master"),
                    Status = 0,
                    Email = "xxx@163.com",
                    DisplayName = "master",
                    Mobile = "11111111",
                    RegistrationTime = DateTime.Now
                };
                Add(user);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
            }
        }

        public bool Exist(string userName) { return CurrentRepository.Exist(u => u.UserName == userName); }

        public User Find(int userId) { return CurrentRepository.Find(u => u.UserId == userId); }

        public User Find(string userName) { return CurrentRepository.Find(u => u.UserName == userName); }

        public IQueryable<User> FindPageList(int pageIndex, int pageSize, out int totalRecord, int order)
        {
            bool isAsc = true;
            string orderName;
            switch (order)
            {
                case 0:
                    isAsc = true;
                    orderName = "UserID";
                    break;
                case 1:
                    isAsc = false;
                    orderName = "UserID";
                    break;
                case 2:
                    isAsc = true;
                    orderName = "RegistrationTime";
                    break;
                case 3:
                    isAsc = false;
                    orderName = "RegistrationTime";
                    break;
                case 4:
                    isAsc = true;
                    orderName = "LoginTime";
                    break;
                case 5: isAsc = false;
                    orderName = "LoginTime";
                    break;
                default:
                    isAsc = false;
                    orderName = "UserID";
                    break;
            }
            return CurrentRepository.FindPageList(pageIndex, pageSize, out totalRecord, u => true, orderName, isAsc);
        }
    }
}
