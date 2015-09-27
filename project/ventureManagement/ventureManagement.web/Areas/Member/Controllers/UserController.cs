using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Areas.Member.Controllers
{
    [AccessDeniedAuthorize(Roles = Role.PERIMISSION_USER_WRITE+","+Role.PERIMISSION_ORGANIZATION_READ)]
    public class UserController : MemberBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read(StoreRequestParameters parameters)
        {
            return this.Store(UsersPaging(parameters));
        }

        private Paging<User> UsersPaging(StoreRequestParameters parameters)
        {
            return UserPaging(parameters.Start, parameters.Limit, parameters.SimpleSort, parameters.SimpleSortDirection, null);
        }

        private Paging<User> UserPaging(int start, int limit, string sort, SortDirection dir, string filter)
        {
            var pageIndex = start/limit + ((start%limit > 0) ? 1 : 0) + 1;
            var count = 0;
            var users = _userService.FindPageList(pageIndex, limit, out count,0).ToList();

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                users.RemoveAll(user => !user.DisplayName.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                users.Sort(delegate(User x, User y)
                {
                    var direction = dir == SortDirection.DESC ? -1 : 1;

                    var userA = x.GetType().GetProperty(sort).GetValue(x, null);
                    var userB = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(userA, userB) * direction;
                });
            }

            return new Paging<User>(users, count);
        }

        public DirectResult Edit(int id, string field, string oldValue, string newValue, object customer)
        {
            string message = "<b>Property:</b> {0}<br /><b>Field:</b> {1}<br /><b>Old Value:</b> {2}<br /><b>New Value:</b> {3}";

            // Send Message...
            X.Msg.Notify(new NotificationConfig()
            {
                Title = "Edit Record #" + id.ToString(),
                Html = string.Format(message, id, field, oldValue, newValue),
                Width = 250
            }).Show();

            X.GetCmp<Store>("UserGridStore").GetById(id).Commit();

            return this.Direct();
        }

        public ActionResult GetUsers(int? organizationId, int start, int limit, int page, string query)
        {
            IQueryable<User> users = null;
            if (organizationId == null)
            {
                users = _userService.FindList(u => true, "UserId", false);
            }
            else
            {
                users = _userService.FindList(u => u.UserOrganizationRelations.Any(uor => uor.OrganizationId == (int)organizationId),
                    "UserId", false);
            }
            
            var pagingUsers = new Paging<User>(users, users.Count());
            return this.Store(pagingUsers.Data, pagingUsers.TotalRecords);
        }

        public ActionResult UpdateUsers(StoreDataHandler handler)
        {
            var users = handler.BatchObjectData<User>();

            var store = this.GetCmp<Store>("UserGridStore");

            foreach (var createdUser in users.Created)
            {
                if(string.IsNullOrEmpty(createdUser.UserName) ||
                    string.IsNullOrEmpty(createdUser.Email) ||
                    string.IsNullOrEmpty(createdUser.Mobile) ||
                    string.IsNullOrEmpty(createdUser.DisplayName))
                {
                    var record = store.GetById(createdUser.UserId);
                    X.Msg.Alert("", "用户名/真名/邮箱/手机号不能为空，请重试").Show();
                    record.Reject();
                    return this.Direct();
                }

                var user = _userService.Find(createdUser.UserName);

                if (user != null)
                {
                    var record = store.GetById(createdUser.UserId);
                    X.Msg.Alert("", "用户名冲突，请重试").Show();
                    record.Destroy();
                    return this.Direct();
                }

                user = new User();
                user = createdUser;
                user.RegistrationTime = DateTime.Now;
                user.Password = Common.Utility.DesEncrypt(user.UserName);

                // ReSharper disable once InvertIf
                try
                {
                    if ((user = _userService.Add(user)) != null)
                    {
                        var record = store.Find("UserName",createdUser.UserName);
                        record.Set("UserId", user.UserId);
                        record.Commit();
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Print(ex.Message);
                }
                
            }

            foreach (var updatedUser in users.Updated)
            {
                var user = _userService.Find(updatedUser.UserId);

                if (user.UserName != updatedUser.UserName)
                {
                    var record = store.GetById(updatedUser.UserId);
                    record.Reject();
                    X.Msg.Alert("", "用户名不能更改").Show();
                    return this.Direct();
                }

                if (string.IsNullOrEmpty(updatedUser.DisplayName) ||
                    string.IsNullOrEmpty(updatedUser.Email) ||
                    string.IsNullOrEmpty(updatedUser.Mobile))
                {
                    var record = store.GetById(updatedUser.UserId);
                    X.Msg.Alert("", "真名/邮箱/手机号不能为空，请重试").Show();
                    record.Reject();
                    return this.Direct();
                }

                user.DisplayName = updatedUser.DisplayName;
                user.Email = updatedUser.Email;
                user.Mobile = updatedUser.Mobile;
                user.Status = updatedUser.Status;

                if (_userService.Update(user))
                {
                    var record = store.GetById(updatedUser.UserId);
                    record.Commit();
                }
            }

            foreach (var deletedUser in users.Deleted)
            {
                if (null == _userService.Find(deletedUser.UserName))
                {
                    store.CommitRemoving(deletedUser.UserId);
                    continue;
                }

                if (_userRoleRelationService.DeleteByUser(deletedUser.UserName))
                {
                    if (_userService.Delete(_userService.Find(deletedUser.UserName)))
                    {
                        store.CommitRemoving(deletedUser.UserId);
                    }
                }
            }

            return this.Direct();
        }
    }
}
