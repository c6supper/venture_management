using ventureManagement.IDAL;
using ventureManagement.DAL;
using ventureManagement.IBLL;
using ventureManagement.models;
using System.Linq;
using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;

namespace ventureManagement.BLL
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
                    Password = Common.Utility.DesEncrypt("master"),
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
                DbEntityValidationException dataEx = (DbEntityValidationException) ex;
                Debug.Print(ex.StackTrace);
            }
        }

        public bool Exist(string userName) { return CurrentRepository.Exist(u => u.UserName == userName); }

        public User Find(int userId) { return CurrentRepository.Find(u => u.UserId == userId); }

        public User Find(string userName) { return CurrentRepository.Find(u => u.UserName == userName); }

        public IQueryable<User> FindPageList(int pageIndex, int pageSize, out int totalRecord, int order)
        {
            bool _isAsc = true;
            string _orderName = string.Empty;
            switch (order)
            {
                case 0:
                    _isAsc = true;
                    _orderName = "UserID";
                    break;
                case 1:
                    _isAsc = false;
                    _orderName = "UserID";
                    break;
                case 2:
                    _isAsc = true;
                    _orderName = "RegistrationTime";
                    break;
                case 3:
                    _isAsc = false;
                    _orderName = "RegistrationTime";
                    break;
                case 4:
                    _isAsc = true;
                    _orderName = "LoginTime";
                    break;
                case 5: _isAsc = false;
                    _orderName = "LoginTime";
                    break;
                default:
                    _isAsc = false;
                    _orderName = "UserID";
                    break;
            }
            return CurrentRepository.FindPageList(pageIndex, pageSize, out totalRecord, u => true, _orderName, _isAsc);
        }
    }
}
