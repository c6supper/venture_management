using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Common;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.BLL;
using VentureManagement.DAL;
using VentureManagement.IDAL;

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
        private readonly HashSet<int> _orgHash;
        public UserService(HashSet<int> orgHash)
            : base(RepositoryFactory.UserRepository)
        {
            _orgHash = orgHash;
            if (_orgHash != null)
            {
                CurrentRepository.EntityFilterEvent += UserFilterEvent;                
            }
        }

        public UserService()
            : base(RepositoryFactory.UserRepository)
        {
        }

        private object UserFilterEvent(object sender, FileterEventArgs e)
        {
            var users = e.EventArg as IQueryable<User>;
            return users != null ? users.Where(u => _orgHash.Contains(u.OrganizationId)) : null;
        }

        public override bool Initilization()
        {
            try
            {
                if (Find(User.USER_ADMIN) != null) return true;

                var user = new User
                {
                    UserName = User.USER_ADMIN,
                    Password = Utility.DesEncrypt(User.USER_ADMIN),
                    Status = User.STATUS_VALID,
                    Email = "xxx@163.com",
                    DisplayName = User.USER_ADMIN,
                    Mobile = "17608007325",
                    RegistrationTime = DateTime.Now,
                    OrganizationId = 1,
                    RoleId = 1
                };
                Add(user);

#if DEBUG
                user = new User
                {
                    UserName = "reporter",
                    Password = Utility.DesEncrypt("reporter"),
                    Status = User.STATUS_VALID,
                    Email = "xxx@163.com",
                    DisplayName = "reporter",
                    Mobile = "17608007325",
                    RegistrationTime = DateTime.Now,
                    OrganizationId = 3,
                    RoleId = 2
                };
                Add(user);
                user = new User
                {
                    UserName = "projectOwner",
                    Password = Utility.DesEncrypt("projectOwner"),
                    Status = User.STATUS_VALID,
                    Email = "xxx@163.com",
                    DisplayName = "projectOwner",
                    Mobile = "17608007325",
                    RegistrationTime = DateTime.Now,
                    OrganizationId = 4,
                    RoleId = 5
                };
                Add(user);
                user = new User
                {
                    UserName = "confirmer",
                    Password = Utility.DesEncrypt("confirmer"),
                    Status = User.STATUS_VALID,
                    Email = "xxx@163.com",
                    DisplayName = "confirmer",
                    Mobile = "17608007325",
                    RegistrationTime = DateTime.Now,
                    OrganizationId = 3,
                    RoleId = 3
                };
                Add(user);
                user = new User
                {
                    UserName = "reviewer",
                    Password = Utility.DesEncrypt("reviewer"),
                    Status = User.STATUS_VALID,
                    Email = "xxx@163.com",
                    DisplayName = "reviewer",
                    Mobile = "17608007325",
                    RegistrationTime = DateTime.Now,
                    OrganizationId = 3,
                    RoleId = 2
                };
                Add(user);
#endif

                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                return false;
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

        public IQueryable<User> FindList(Expression<Func<User, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }
    }
}
