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
        private readonly List<int> _currentOrgList;
        public UserService(List<int> currentOrgList)
            : base(RepositoryFactory.UserRepository)
        {
            _currentOrgList = currentOrgList;
            if (currentOrgList != null)
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
            Debug.Assert(users != null, "users != null");

            var filteredUsers = new List<User>();
            foreach (var orgId in _currentOrgList)
            {
                filteredUsers.AddRange(users.Where(u=>u.UserOrganizationRelations.Any(uorgr=>uorgr.OrganizationId == orgId)));
            }

            return filteredUsers.AsQueryable();
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
                    RegistrationTime = DateTime.Now
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
                    RegistrationTime = DateTime.Now
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
                    RegistrationTime = DateTime.Now
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
                    RegistrationTime = DateTime.Now
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
                    RegistrationTime = DateTime.Now
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

        public int Count()
        {
            return CurrentRepository.Count(u => true);
        }

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

        public bool Add(User user,int roleId,int orgId)
        {
            using (var transaction = CurrentRepository.BeginTransaction())
            {
                try
                {
                    var userRoleRelationService = new UserRoleRelationService();
                    var userOrgRelationService = new UserOrganizationRelationService();
                    if (Add(user) != null)
                    {
                        var urr = new UserRoleRelation
                        {
                            UserId = user.UserId,
                            RoleId = roleId
                        };
                        if (userRoleRelationService.Add(urr) != null)
                        {
                            var uor = new UserOrganizationRelation
                            {
                                UserId = user.UserId,
                                OrganizationId = orgId
                            };
                            if (userOrgRelationService.Add(uor) != null)
                            {
                                transaction.Commit();
                                return true;
                            }
                        }
                    }
                    transaction.Rollback();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.Print(ex.Message);
                    return false;
                }
            }
            return false;
        }
    }
}
