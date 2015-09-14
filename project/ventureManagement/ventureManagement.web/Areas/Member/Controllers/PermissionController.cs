using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.IBLL;
using VentureManagement.BLL;
using VentureManagement.Models;

namespace VentureManagement.Web.Areas.Member.Controllers
{
    [Authorize(Roles = Role.PERIMISSION_PERMISSION_READ+","+Role.PERIMISSION_PERMISSION_WRITE)]
    public class PermissionController : Controller
    {
        //
        // GET: /Member/Permission/

        private readonly InterfaceRoleService _roleService = new VentureManagement.BLL.RoleService();

        public ActionResult Index()
        {
            return View(PermissionPaging());
        }

        public ActionResult GetData()
        {
            return this.Store(PermissionPaging());
        }

        private IEnumerable PermissionPaging()
        {
            var permissions = new List<object>();

            foreach (var role in _roleService.FindList(r => true,"RoleId",false))
            {
                permissions.Add(role.RoleValueToAllPermissions());
            }

            return permissions;
        }

        public ActionResult UpdatePermissions(StoreDataHandler handler)
        {
            var roles = handler.BatchObjectData<Role>();

            var store = this.GetCmp<Store>("UserGridStore");

            foreach (var createdUser in users.Created)
            {
                if (string.IsNullOrEmpty(createdUser.UserName) ||
                    string.IsNullOrEmpty(createdUser.Email) ||
                    string.IsNullOrEmpty(createdUser.Mobile) ||
                    string.IsNullOrEmpty(createdUser.DisplayName))
                {
                    var record = store.GetById(createdUser.UserId);
                    X.Msg.Alert("", "用户名/昵称/邮箱/手机号不能为空，请重试").Show();
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
                    if (_userService.Add(user) != null)
                    {
                        var record = store.Find("UserName", createdUser.UserName);
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
                    X.Msg.Alert("", "昵称/邮箱/手机号不能为空，请重试").Show();
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
