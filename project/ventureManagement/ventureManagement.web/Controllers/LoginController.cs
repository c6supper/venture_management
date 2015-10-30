using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web.Mvc;
using System.Web.Security;
using Common;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Controllers
{
    [DirectController(AreaName = "Member", GenerateProxyForOtherControllers = false, IDMode = DirectMethodProxyIDMode.None)]
    public class LoginController : BaseController
    {
        readonly InterfaceUserService _userService = new UserService();
        readonly InterfaceOrganizationService _orgService = new OrganizationService();
        readonly InterfaceOrganizationRelationService _orgrService = new OrganizationRelationService();
        readonly InterfaceUserOrganizationRelationService _uorgService = new UserOrganizationRelationService();

        [AllowAnonymous]
        public ActionResult Index()
        {
            if(!Request.IsAuthenticated)
                return View();

            return RedirectToAction("Index","Main");
        }

        [AllowAnonymous]
        public ActionResult Login(string txtUsername, string txtPassword, string verificationCode, string returnUrl)
        {
#if DEBUG
            if (string.IsNullOrEmpty(txtUsername))
                txtUsername = "master";

            var user = _userService.Find(txtUsername);
            txtPassword = Common.Utility.DesDecrypt(user.Password);
#endif

#if DEBUG
#else
            if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != verificationCode.ToUpper())
            {
                X.Msg.Alert("", "验证码错误，请重试").Show();
                return this.Direct();
            }
#endif

            // Validate the user login.
            if (Membership.ValidateUser(txtUsername, txtPassword))
            {
                // Create the authentication ticket.
                FormsAuthentication.SetAuthCookie(txtUsername, false);

                var currentUser = _userService.Find(txtUsername);
                if (currentUser.Status != VentureManagement.Models.User.STATUS_VALID)
                {
                    X.Msg.Alert("提示", string.Concat("此用户名", currentUser.Status, "，请联系管理员")).Show();
                    return this.Direct();
                }
                Session["UserId"] = currentUser.UserId;

                var orgs = _uorgService.FindList(txtUsername).Select(uorg=>uorg.Organization).ToList();
                Session["Organization"] = orgs;
                var currentOrgList = new List<int>();
                foreach (var org in orgs)
                {
                    currentOrgList.Add(org.OrganizationId);
                    currentOrgList.AddRange(_orgrService.GetChildrenOrgList(org.OrganizationName));
                }
                Session["currentOrgList"] = currentOrgList;

                //login time/ip
                currentUser.LoginTime = DateTime.Now;
                currentUser.LoginIP = GetLocalIP();
                _userService.Update(currentUser);

                // Redirect to the secure area.
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToRoute("Default", new { Controller = "Main", Action = "Index" }); ;
                }
            }
            else
            {
                X.Msg.Alert("提示", "用户名或密码错误,请重新输入").Show();
            }

            return this.Direct();
        }

        [AllowAnonymous]
        private string GetLocalIP()
        {
            string userIP = "未获取用户IP";

            try
            {
                if (System.Web.HttpContext.Current == null
                    || System.Web.HttpContext.Current.Request == null
                    || System.Web.HttpContext.Current.Request.ServerVariables == null)
                    return "";

                string CustomerIP = "";

                //CDN加速后取到的IP 
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(CustomerIP))
                    return CustomerIP;

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (CustomerIP == null)
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                }

                if (string.Compare(CustomerIP, "unknown", true) == 0)
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                return CustomerIP;
            }
            catch { }

            return userIP;
        }
        
        [AllowAnonymous]
        public ActionResult Register()
        {          
            return RedirectToAction("Index", "Register");
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            // Delete the user details from cache.
            System.Web.HttpContext.Current.Cache.Remove(User.Identity.Name);

            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Main");
        }

        [AllowAnonymous]
        public ActionResult VerificationCode()
        {
            string verificationCode = Security.CreateVerificationText(4);
            var _img = Security.CreateVerificationImage(verificationCode, 160, 30);
            _img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            return null;
        }
    }
}
