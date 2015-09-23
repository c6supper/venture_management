using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            /*
            var orgController = new OrganizationController();
            var orgs = orgController.GetOrganizations(start, limit, page, query);
            return this.Store(orgs.Data, orgs.TotalRecords);
            */
            return this.Store(null);
        }

        [AllowAnonymous]
        public ActionResult GetAllRoles()
        {
            //TODO:
            
            return this.Store(null);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateUser(string userName, string displayName,string userPassword, string userConfirmPassword,
            string userEmail, string userMobile, int? OrganizationId, int? RoleId)
        {
            if (string.IsNullOrEmpty(userName)
                || string.IsNullOrEmpty(displayName)
                || string.IsNullOrEmpty(userPassword)
                || string.IsNullOrEmpty(userConfirmPassword)
                || string.IsNullOrEmpty(userEmail)
                || string.IsNullOrEmpty(userMobile))
            {
                X.Msg.Alert("", "用户名/昵称/密码/邮箱/手机号不能为空，请重试").Show();
                return this.Direct();
            }
            
            if (userPassword != userConfirmPassword)
            {
                X.Msg.Alert("", "确认密码错误，请重试").Show();
                return this.Direct();
            }

            //var user = _userService.Find(userName);
            UserService us = new UserService();
            var user = us.Find(userName);
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
                //if ((user = _userService.Add(user)) == null)
                if ((user = us.Add(user)) == null)
                {
                    X.Msg.Alert("", "用户名注册失败，请重试").Show();
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            UserRoleRelation urr = new UserRoleRelation();
            urr.UserId = user.UserId;
            urr.RoleId = 1;//(int)RoleId;
            //_userRoleRelationService.Add(urr);
            UserRoleRelationService urrs = new UserRoleRelationService();
            urrs.Add(urr);
            
            UserOrganizationRelation uor = new UserOrganizationRelation();
            uor.UserId = user.UserId;
            uor.OrganizationId = 1;//(int)OrganizationId;
            //_userOrgRelationService.Add(uor);
            UserOrganizationRelationService uors = new UserOrganizationRelationService();
            uors.Add(uor);

            X.Msg.Alert("", "用户名注册成功，请登录").Show();
            
            return RedirectToAction("Index", "Main");
        }

        [AllowAnonymous]
        public ActionResult Cancel()
        {
            return RedirectToAction("Index", "Main");
        }
    }
}
