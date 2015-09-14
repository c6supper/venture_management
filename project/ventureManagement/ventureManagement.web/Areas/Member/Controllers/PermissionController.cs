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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VentureManagement.IDAL;

namespace VentureManagement.Web.Areas.Member.Controllers
{
    [Authorize(Roles = Role.PERIMISSION_PERMISSION_READ+","+Role.PERIMISSION_PERMISSION_WRITE)]
    public class PermissionController : Controller
    {
        //
        // GET: /Member/Permission/

        private readonly InterfaceRoleService _roleService = new VentureManagement.BLL.RoleService();
        private readonly InterfaceRoleRelationService _userRoleRelationService = new UserRoleRelationService();
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

        private BitArray ParseJsonToPermission(JObject jsons)
        {
            var bitArray = new BitArray(new int[]{0,0});
            var permissionStrings = Role.PermissionStrings.ToList();

            foreach (var json in jsons)
            {
                if (permissionStrings.Contains(json.Key))
                {
                    bitArray.Set(permissionStrings.IndexOf(json.Key), jsons[json.Key].ToObject<bool>());
                }
            }

            return bitArray;
        }

        public ActionResult UpdatePermissions(StoreDataHandler handler)
        {
            var roles = handler.BatchObjectData<Role>();
            var jsonResult = JsonConvert.DeserializeObject<JObject>(handler.JsonData);

            var store = this.GetCmp<Store>("UserGridStore");

            foreach (var createdRole in roles.Created)
            {
                if(!TryValidateModel(createdRole))
                {
                    var record = store.GetById(createdRole.RoleId);
                    X.Msg.Alert("", "角色名（不少于两个字）/备注/不能为空，请重试").Show();
                    record.Reject();
                    return this.Direct();
                }

                var role = _roleService.Find(createdRole.RoleName);

                if (role != null)
                {
                    var record = store.Find("RoleName", createdRole.RoleName);
                    X.Msg.Alert("", "角色名冲突，请重试").Show();
                    record.Destroy();
                    return this.Direct();
                }

                role = new Role();
                role = createdRole;
                role.PermissionsToRoleValue(ParseJsonToPermission(jsonResult["Created"].ToObject<JArray>()[0].ToObject<JObject>()));
                // ReSharper disable once InvertIf
                try
                {
                    if (_roleService.Add(role) != null)
                    {
                        var record = store.Find("RoleName", role.RoleName);
                        record.Commit();
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Print(ex.Message);
                }
            }

            foreach (var updatedRole in roles.Updated)
            {
                var role = _roleService.Find(updatedRole.RoleId);

                if (role.RoleName != updatedRole.RoleName)
                {
                    var record = store.GetById(updatedRole.RoleId);
                    record.Reject();
                    X.Msg.Alert("", "角色名不能更改").Show();
                    return this.Direct();
                }

                if (!TryValidateModel(updatedRole))
                {
                    var record = store.GetById(updatedRole.RoleId);
                    X.Msg.Alert("", "角色名（不少于两个字）/备注/不能为空，请重试").Show();
                    record.Reject();
                    return this.Direct();
                }

                role.Description = updatedRole.Description;
                role.RoleName = updatedRole.RoleName;
                role.PermissionsToRoleValue(ParseJsonToPermission(jsonResult["Updated"].ToObject<JArray>()[0].ToObject<JObject>()));

                if (_roleService.Update(role))
                {
                    var record = store.GetById(role.RoleId);
                    record.Commit();
                }
            }

            foreach (var deletedRole in roles.Deleted)
            {
                if (null == _roleService.Find(deletedRole.RoleId))
                {
                    store.CommitRemoving(deletedRole.RoleId);
                    continue;
                }

                if (_userRoleRelationService.DeleteByRole(deletedRole.RoleId))
                {
                    if (_roleService.Delete(_roleService.Find(deletedRole.RoleId)))
                    {
                        store.CommitRemoving(deletedRole.RoleId);
                    }
                }
            }

            return this.Direct();
        }

    }
}
