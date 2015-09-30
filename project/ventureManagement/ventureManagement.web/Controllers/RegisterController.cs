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
            return View();
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

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateUser(string userName, string displayName,string userPassword, string userConfirmPassword,
            string userEmail, string userMobile, int? OrganizationId, int? RoleId, string verificationCode)
        {
            if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != verificationCode.ToUpper())
            {
                X.Msg.Alert("", "验证码错误，请重试").Show();
                return this.Direct();
            }

            if (string.IsNullOrEmpty(userName)
                || string.IsNullOrEmpty(displayName)
                || string.IsNullOrEmpty(userPassword)
                || string.IsNullOrEmpty(userConfirmPassword)
                || string.IsNullOrEmpty(userEmail)
                || string.IsNullOrEmpty(userMobile)
                || (OrganizationId == null)
                || (RoleId == null))
            {
                X.Msg.Alert("", "用户名/实名/密码/邮箱/手机号/部门/角色不能为空，<br/>请重试").Show();
                return this.Direct();
            }
            
            if (userPassword != userConfirmPassword)
            {
                X.Msg.Alert("", "确认密码错误，请重试").Show();
                return this.Direct();
            }

            var user = _userService.Find(userName);
            if (user != null)
            {
                X.Msg.Alert("", "用户名冲突，请重试").Show();
                return this.Direct();
            }

            user = new User();
            user.UserName = userName;
            user.DisplayName = displayName;
            user.Password = Common.Utility.DesEncrypt(userPassword);
            user.Email = userEmail;
            user.Mobile = userMobile;
            user.RegistrationTime = DateTime.Now;
            user.Status = VentureManagement.Models.User.STATUS_INVALID;

            try
            {
                if ((user = _userService.Add(user)) == null)
                {
                    X.Msg.Alert("", "用户名注册失败，请重试").Show();
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            var urr = new UserRoleRelation();
            urr.UserId = user.UserId;
            urr.RoleId = (int)RoleId;
            _userRoleRelationService.Add(urr);
            
            var uor = new UserOrganizationRelation();
            uor.UserId = user.UserId;
            uor.OrganizationId = (int)OrganizationId;
            _userOrgRelationService.Add(uor);

            X.Msg.Confirm("提示", "用户名注册成功，请登录", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "document.location.href='login';",
                    Text = "确定"
                }
            }).Show();

            return this.Direct();
        }

        [AllowAnonymous]
        public ActionResult Cancel()
        {
            return RedirectToAction("Index", "Main");
        }

        [AllowAnonymous]
        public ActionResult VerificationCode()
        {
            string verificationCode = Security.CreateVerificationText(6);
            Bitmap _img = Security.CreateVerificationImage(verificationCode, 160, 30);
            _img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            return null;
        }
    }
}
