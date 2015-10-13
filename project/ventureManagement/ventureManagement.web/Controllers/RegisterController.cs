using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.Models;
using VentureManagement.Web.Areas.Member.Controllers;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Controllers
{
    public class RegisterController : MemberBaseController//Controller
    {
        //
        // GET: /Register/

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(new User());
        }

        [AllowAnonymous]
        public ActionResult GetAllOrganizations(int start, int limit, int page, string query)
        {
            var orgController = new OrganizationController();
            var orgs = orgController.GetOrganizations(start, limit, page, query);

            return this.Store(orgs.Data, orgs.TotalRecords);
        }

        [AllowAnonymous]
        public ActionResult GetAllRoles(int start, int limit, int page, string query)
        {
            var permissionController = new PermissionController();
            var roles = permissionController.GetRoles(start, limit, page, query);

            foreach (var role in roles.Data)
            {
                if (role.RoleName == Role.ROLE_ADMIN)
                {
                    roles.Data.Remove(role);
                    break;
                }   
            }

            return this.Store(roles.Data, roles.TotalRecords);
        }

        [AllowAnonymous]
        public ActionResult VerificationCode()
        {
            string verificationCode = Security.CreateVerificationText(6);
            var _img = Security.CreateVerificationImage(verificationCode, 160, 30);
            _img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            return null;
        }

        [AllowAnonymous]
        public ActionResult Submit(User user, string verificationCode, string roleId,string orgId)
        {
            try
            {
                if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != verificationCode.ToUpper())
                {
                    X.Msg.Alert("", "验证码错误，请重试").Show();
                    return this.FormPanel();
                }

                ModelState.Clear();
                if (!TryValidateModel(user))
                {
                    X.Msg.Alert("", "用户名/实名/密码/邮箱/手机号/部门/角色不能为空，<br/>请检查输入参数").Show();
                    return this.FormPanel();
                }

                // ReSharper disable once AccessToModifiedClosure
                if (_userService.FindList(u => u.UserName == user.UserName || u.Mobile == user.Mobile, "UserId", false).Any())
                {
                    X.Msg.Alert("", "用户名/手机号冲突，请检查输入参数").Show();
                    return this.FormPanel();
                }

                user.Password = Utility.DesEncrypt(user.Password);
                user.RegistrationTime = DateTime.Now;
                user.Status = VentureManagement.Models.User.STATUS_INVALID;

                if (!_userService.Add(user, Convert.ToInt32(roleId), Convert.ToInt32(orgId)))
                {
                    X.Msg.Alert("", "用户注册失败，请检查输入参数").Show();
                    return this.FormPanel();
                }

                X.Msg.Confirm("提示", "用户注册成功,待管理员审核", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "document.location.href='login';",
                        Text = "确定"
                    }
                }).Show();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                X.Msg.Alert("", "用户注册失败，请检查输入参数").Show();
                return this.FormPanel();
            }

            return this.FormPanel();
        }
    }
}
